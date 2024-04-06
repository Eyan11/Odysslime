using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class BridgeExtend : MonoBehaviour
{
    [SerializeField] private GameObject movingBridge;
    [SerializeField] private Transform bridgeStart;
    [SerializeField] private Transform bridgeEnd;
    [SerializeField] private float extendSpeed;
    [SerializeField] private PlatformMoveSlime moveSlimeScript;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isActivated = false;
    private Vector3 direction;

    private void Awake() {
        //find the length of the bridge
        float bridgeLength = Vector3.Distance(bridgeStart.position, bridgeEnd.position);

        //get end position based on average position of start/end platforms (in-between them)
        endPos = (bridgeStart.position + bridgeEnd.position)/2;
        //get initial poistion of bridge
        startPos = endPos - ((endPos - bridgeStart.position) * 2);

        //adjust position and scale of bridge
        movingBridge.transform.position = startPos;
        movingBridge.transform.localScale = new Vector3(bridgeStart.localScale.x, bridgeStart.localScale.y/2, bridgeLength);

        //Bake NavMesh just on platform now that we know position and scale
        movingBridge.GetComponent<NavMeshSurface>().BuildNavMesh();

        direction = endPos - startPos;
    }

    private void Update() {
        
        //move to end position
        if(isActivated) {
            movingBridge.transform.position = Vector3.MoveTowards(movingBridge.transform.position, endPos, extendSpeed);
            //moveSlimeScript.UpdateAgentPosition(direction, extendSpeed);
        }
        //move to start position
        else {
            movingBridge.transform.position = Vector3.MoveTowards(movingBridge.transform.position, startPos, extendSpeed);
            //moveSlimeScript.UpdateAgentPosition(-direction, extendSpeed);
        }

    }

    public void Activate() {
        isActivated = true;
    }

    public void Deactivate() {
       isActivated = false;
    }
}
