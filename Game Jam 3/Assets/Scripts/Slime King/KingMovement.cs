using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : SlimeMovement
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform kingObj;
    private DiscoverSlimes discoverSlimesScript;
    private Rigidbody rb;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxDistToSlime;
    private Transform trackedSlime = null;
    private float xInput;
    private float zInput;
    private float yInput;
    private Vector3 moveDir;


    private void Awake() {
        //grab rigidbody reference and make sure body doesn't fall over
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        discoverSlimesScript = GetComponent<DiscoverSlimes>();
    }

    private void FixedUpdate() {
        GetInput();
        ConstrainMovement();

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
    }

    private void ConstrainMovement() {

        //find direction of input based on player orientation (relative to camera)
        moveDir = (orientation.forward * zInput) + (kingObj.up * yInput) + (orientation.right * xInput);

        //if tracked slime does NOT exist
        if(trackedSlime == null) {
            //check if there are any other slime followers, if so track it
            if(discoverSlimesScript.GetSlimeFollower() != null)
                trackedSlime = discoverSlimesScript.GetSlimeFollower().transform;
            //if no slime followers, do NOT restrict movement
            else
                return;
        }

        //if the direction the king is moving in is too far from slimes, don't move
        if(Vector3.Distance((transform.position + (moveDir*3)), trackedSlime.position) > maxDistToSlime)
            moveDir = Vector3.zero;
    }

    private void OnDisable() {
        // turns off movement (so the king slime doesn't drift off if already moving)
        rb.velocity = Vector3.zero;
    }

}
