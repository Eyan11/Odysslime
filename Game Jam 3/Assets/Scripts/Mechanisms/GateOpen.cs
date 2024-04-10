using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MechanismBase
{
    [SerializeField] private GameObject gate;
    [SerializeField] private float slideSpeed;
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

        //move to end position
        if(isActivated)
            gate.transform.position = Vector3.MoveTowards(gate.transform.position, endPos, slideSpeed);
        //move to start position
        else
            gate.transform.position = Vector3.MoveTowards(gate.transform.position, startPos, slideSpeed);
    }

    public void Activate() {
        isActivated = true;
    }

    public void Deactivate() {
        isActivated = false;
    }
}
