using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour
{

    [SerializeField] private GameObject gate;
    [SerializeField] private float slideSpeed;
    private float lerpPercent = 0f;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isActivated = false;

    private void Awake() {
        //get initial poistion of gate
        startPos = gate.transform.position;
        //get end position based on average position of static walls (in-between them)
        endPos = (gameObject.transform.GetChild(1).position + gameObject.transform.GetChild(2).position)/2;
    
    }

    private void Update() {

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

    }

    public void Activate() {
        isActivated = true;
    }

    public void Deactivate() {
        isActivated = false;
    }
}
