using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    private Transform king;
    private NavMeshAgent agent;

    [Header("Settings")]
    [SerializeField] private float updateFollowTime = 1f;
    private float updateFollowCounter = 0;
    private Rigidbody rb;

    private void Awake() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        updateFollowCounter = updateFollowTime;
        rb = GetComponent<Rigidbody>();

        king = GameObject.FindObjectOfType<KingMovement>().gameObject.transform;
        this.enabled = false;
    }

    private void Update() {
        updateFollowCounter -= Time.deltaTime;

        //calculates direction to king (vector on XZ plane from slime to king)
        Vector3 viewDir = king.position - new Vector3(transform.position.x, king.position.y, transform.position.z);
        //sets the forward direction to the vector calculated (forward is the direction slime is moving)
        orientation.forward = viewDir.normalized;
    }

    private void FixedUpdate() {
        //only update follow target periodically
        if(updateFollowCounter < 0)
            FollowKing();
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
