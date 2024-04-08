using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float updateFollowTime = 1f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform slimeObjTran;
    private float updateFollowCounter = 0;
    private Transform king;
    private NavMeshAgent agent;
    private Vector3 kingDir;
    private Quaternion rotation;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Quaternion lookRotation;


    private void Awake() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        updateFollowCounter = updateFollowTime;
        rb = GetComponent<Rigidbody>();
        king = GameObject.FindObjectOfType<KingMovement>().gameObject.transform;

        //disable agent rotation so I can do it manually
        agent.updateRotation = false;
        //disable script to make all slimes "undiscovered" at start
        this.enabled = false;
    }

    private void Update() {
        updateFollowCounter -= Time.deltaTime;

        //only update follow target periodically
        if(updateFollowCounter < 0)
            FollowKing();

        //calculates direction to king (vector on XZ plane from slime to king)
        kingDir = king.position - transform.position;
        kingDir.y = 0;

        //smoothly change (Slerp) player orientation to match the input direction
        //rotation = Quaternion.LookRotation(kingDir);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        
        moveDirection = (agent.steeringTarget - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
        slimeObjTran.rotation = Quaternion.Slerp(slimeObjTran.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        
    
    }

    private void FollowKing() {
        updateFollowCounter = updateFollowTime;

        agent.destination = king.position;
    }

    private void OnDisable() {
        agent.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnEnable() {
        agent.enabled = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }


}
