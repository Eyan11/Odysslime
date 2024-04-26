using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

public class PlatformMoveSlime : MonoBehaviour
{
    [SerializeField] private float slimeSpeedMultiplier;

    [Header ("Only For Bridge Mechanism: ")]
    [SerializeField] private Transform exitBridgeDest;
    [SerializeField] private OffMeshLink enterBridgeLink;
    private Dictionary<NavMeshAgent, Transform> agentsOnPlatform = new Dictionary<NavMeshAgent, Transform>();
    private bool exitBridge = false;


    private void OnTriggerEnter(Collider other) {

        //if the object has a NavMesh Agent component, add it to the list
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)) {
            agentsOnPlatform.Add(agent, agent.transform);
        }
    }

    private void OnTriggerExit(Collider other) {
        
        //if the object has a NavMesh Agent component, remove it from the list
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)) {
            Transform oldParent = agentsOnPlatform[agent];
            agentsOnPlatform.Remove(agent);
            agent.transform.parent = oldParent;
        }
    }

    //only for bridge
    private void OnTriggerStay(Collider other) {

        //if the object has a NavMesh Agent component and the bridge is closing
        if(exitBridge && other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)) {

            //set destination to make slime get off the bridge
            agent.destination = (exitBridgeDest.position - transform.position);
        }
    }

    public void UpdateAgentPosition(Vector3 direction, float speed) {
        //loop through every agent standing on platform
        foreach(KeyValuePair<NavMeshAgent, Transform> pair in agentsOnPlatform) {
            NavMeshAgent agent = pair.Key;
            if (agent == null) { // Just in case the slime wants to die on the platform
                agentsOnPlatform.Remove(agent);
                continue;
            }

            //update their destination to match the movement of the platform
            // do this only if the agent is enabled
            if (agent.enabled) {
                agent.Move(direction * speed * Time.deltaTime * slimeSpeedMultiplier);
            } else { // otherwise, make them a child of the platform temporarily
                agent.gameObject.transform.parent = gameObject.transform;
            }
        }
    }

    //only for bridge
    private void Update() {
        
        if(exitBridge) {

            foreach(KeyValuePair<NavMeshAgent, Transform> pair in agentsOnPlatform) {
                NavMeshAgent agent = pair.Key;
                if (agent == null) {
                    agentsOnPlatform.Remove(agent);
                    continue;
                }

                agent.SetDestination(exitBridgeDest.position - transform.position);
            }
        }
    }

    //only for bridge
    public void SetExitBridge(bool value) {
        exitBridge = value;

        //if bridge is retracting, turn off link to enter bridge
        if(exitBridge)
            enterBridgeLink.activated = false;
        //if bridge is NOT retracting, turn on link to enter bridge 
        else
            enterBridgeLink.activated = true;
    }

}
