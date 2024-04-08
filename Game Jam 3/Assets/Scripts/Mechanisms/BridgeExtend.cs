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
    [SerializeField] private float retractSpeed;
    [SerializeField] private float bufferDistance;
    [SerializeField] private PlatformMoveSlime moveSlimeScript;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isActivated = false;
    private bool isMoving = false;
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

        if(isMoving && isActivated) {
            ExtendBridge();
        }
        else if(isMoving && !isActivated) {
            RetractBridge();
        }
    }

    public void Activate() {
        isActivated = true;
        isMoving = true;
        moveSlimeScript.SetExitBridge(false);
    }

    public void Deactivate() {
       isActivated = false;
       isMoving = true;
    }


    public void ExtendBridge() {

        //move to end position
        movingBridge.transform.position = Vector3.MoveTowards(movingBridge.transform.position, endPos, extendSpeed * Time.deltaTime);
        
        //move slimes with platform
        moveSlimeScript.UpdateAgentPosition(direction, extendSpeed);

        if(Vector3.Distance(moveSlimeScript.transform.position, endPos) < bufferDistance) {
            isMoving = false;
        }
    }

    public void RetractBridge() {

        //move to start position
        movingBridge.transform.position = Vector3.MoveTowards(movingBridge.transform.position, startPos, retractSpeed * Time.deltaTime);

        //move slimes with platform
        moveSlimeScript.UpdateAgentPosition(-direction, retractSpeed);

        moveSlimeScript.SetExitBridge(true);

        if(Vector3.Distance(moveSlimeScript.transform.position, startPos) < bufferDistance) {
            isMoving = true;
            moveSlimeScript.SetExitBridge(false);
        }
    }
}
