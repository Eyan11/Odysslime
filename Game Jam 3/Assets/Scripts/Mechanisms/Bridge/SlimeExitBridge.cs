using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeExitBridge : MonoBehaviour
{
    private bool exitBridge = false;
    [SerializeField] private Transform exitBridgeDest;
    private List<NavMeshAgent> agentsOnPlatform = new List<NavMeshAgent>();


    private void OnTriggerEnter(Collider other) {

        //if the object has a NavMesh Agent component, add it to the list
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)) {
            agentsOnPlatform.Add(agent);
            //Debug.Log("Slime added");
        }
    }

    private void OnTriggerExit(Collider other) {
        
        //if the object has a NavMesh Agent component, remove it from the list
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)) {
            agentsOnPlatform.Remove(agent);
            //Debug.Log("Slime removed");
        }
    }


/*
    private void Update() {
        
        if(exitBridge) {

            foreach(NavMeshAgent agent in agentsOnPlatform) {
                agent.SetDestination(exitBridgeDest.position - transform.position);
            }
        }
    }
*/



/*

    private void OnTriggerStay(Collider other) {

        //if the object has a NavMesh Agent component and the bridge is closing
        if(exitBridge && other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)) {

            //set destination to make slime get off the bridge
            //agent.Move((exitBridgeDest - transform.position) * Time.deltaTime * 0.2f);
            agent.destination = (exitBridgeDest.position - transform.position);
            //Debug.Log("" + agent.name);
        }
    }
*/

    public void SetExitBridge(bool value) {
        exitBridge = value;
    }


}
