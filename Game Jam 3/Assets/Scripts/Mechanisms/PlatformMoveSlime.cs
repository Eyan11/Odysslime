using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlatformMoveSlime : MonoBehaviour
{
    [SerializeField] private float slimeSpeedMultiplier;

    [Header ("Only For Bridge Mechanism: ")]
    [SerializeField] private Transform exitBridgeDest;
    [SerializeField] private OffMeshLink enterBridgeLink;
    private List<NavMeshAgent> agentsOnPlatform = new List<NavMeshAgent>();
    private bool exitBridge = false;


    private void OnTriggerEnter(Collider other) {

        //if the object has a NavMesh Agent component, add it to the list
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)) {
            agentsOnPlatform.Add(agent);
            Debug.Log("Slime added");
        }
    }

    private void OnTriggerExit(Collider other) {
        
        //if the object has a NavMesh Agent component, remove it from the list
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)) {
            agentsOnPlatform.Remove(agent);
            Debug.Log("Slime removed");
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
        foreach(NavMeshAgent agent in agentsOnPlatform) {

            //update their destination to match the movement of the platform
            agent.Move(direction * speed * Time.deltaTime * slimeSpeedMultiplier);
        }
    }

    //only for bridge
    private void Update() {
        
        if(exitBridge) {

            foreach(NavMeshAgent agent in agentsOnPlatform) {
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
