using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeExtend : MonoBehaviour
{
    [SerializeField] private GameObject movingBridge;
    [SerializeField] private Transform bridgeStart;
    [SerializeField] private Transform bridgeEnd;
    [SerializeField] private float extendSpeed;
    private float lerpPercent = 0f;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isActivated = false;

    private void Awake() {
        //get end position based on average position of start/end platforms (in-between them)
        endPos = (bridgeStart.position + bridgeEnd.position)/2;
        //get initial poistion of gate
        startPos = bridgeStart.position - endPos;

        //movingBridge.transform.position = startPos;
        float bridgeLength = Vector3.Distance(startPos, endPos);
        movingBridge.transform.localScale = new Vector3(bridgeStart.localScale.x, bridgeStart.localScale.y/2, bridgeLength);

    }

    private void Update() {
        /*
        //if active and not done opening
        if(isActivated && lerpPercent < 1) {
            //open gate
            gate.transform.position = Vector3.Lerp(gate.transform.position, endPos, lerpPercent);
            lerpPercent += slideSpeed * Time.deltaTime;
        }
        //if NOT active and not done closing
        else if(!isActivated && lerpPercent > 0) {
            //close gate
            gate.transform.position = Vector3.Lerp(gate.transform.position, startPos, lerpPercent);
            lerpPercent -= slideSpeed * Time.deltaTime;
        }
        */

    }

    public void Activate() {
        isActivated = true;
    }

    public void Deactivate() {
        isActivated = false;
    }
}
