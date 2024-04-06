using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlatformMoveSlime : MonoBehaviour
{

    private List<NavMeshAgent> agentsOnPlatform = new List<NavMeshAgent>();


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

    public void UpdateAgentPosition(Vector3 direction, float speed) {

        //loop through every agent standing on platform
        foreach(NavMeshAgent agent in agentsOnPlatform) {

            //update their destination to match the movement of the platform
            agent.destination += direction * speed;
        }
    }
}
