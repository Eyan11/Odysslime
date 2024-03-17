using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : SlimeMovement
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform kingObj;
    
    private float xInput;
    private float zInput;
    private float yInput;
    private Vector3 moveDir;
    private Rigidbody rb;

    private void Awake() {
        //grab rigidbody reference and make sure body doesn't fall over
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update() {
        GetInput();

        //set velocity
        rb.velocity = moveDir * moveSpeed;
    }

    private void GetInput() {
        //get X axis input
        xInput = Input.GetAxisRaw("Horizontal");
        //get z axis input
        zInput = Input.GetAxisRaw("Vertical");

        //get y axis input (space is up, shift is down)
        if(Input.GetKey(KeyCode.Space))
            yInput = 1;
        else if(Input.GetKey(KeyCode.LeftShift))
            yInput = -1;
        else
            yInput = 0;


        //find direction of input based on player orientation (relative to camera)
        moveDir = (orientation.forward * zInput) + (kingObj.up * yInput) + (orientation.right * xInput);
        //y axis moveDir is based on obj because I want king to move up/down relative to the obj not the camera direction
    }

    private void OnDisable() {
        // turns off movement (so the king slime doesn't drift off if already moving)
        rb.velocity = Vector3.zero;
    }

}
