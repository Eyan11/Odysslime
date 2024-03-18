using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : SlimeMovement
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform kingObj;
    private Rigidbody rb;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxDistToSlime;
    [SerializeField] private float maxDistCheckTime;
    private Transform anySlime;
    private float maxDistCheckCounter = 0;
    private float xInput;
    private float zInput;
    private float yInput;
    private Vector3 moveDir;


    private void Awake() {
        //grab rigidbody reference and make sure body doesn't fall over
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        maxDistCheckCounter = maxDistCheckTime;
    }

    private void Update() {
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
        maxDistCheckCounter -= Time.deltaTime;

        //if no slime currently tracked, find another
        if(anySlime == null)
            FindAnySlimeFollower();


        //find direction of input based on player orientation (relative to camera)
        moveDir = (orientation.forward * zInput) + (kingObj.up * yInput) + (orientation.right * xInput);
        //y axis moveDir is based on obj because I want king to move up/down relative to the obj not the camera direction

        //if the direction the king is moving in is too far from slimes, don't move
        if(Vector3.Distance((transform.position + (moveDir*3)), anySlime.position) > maxDistToSlime)
            moveDir = Vector3.zero;
        
    }

    private void FindAnySlimeFollower() {

        //collect all colliders in radius of king
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDistToSlime * 2);

        //loop through each collider in array
        foreach (var hitCollider in hitColliders) {

            //if collider belongs to slime follower, set slime as anySlime
            if(hitCollider.gameObject.CompareTag("Slime Follower")) {
                anySlime = hitCollider.gameObject.transform;
                return;
            }
        }
    }

    private void OnDisable() {
        // turns off movement (so the king slime doesn't drift off if already moving)
        rb.velocity = Vector3.zero;
    }

}
