using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeFollow : MonoBehaviour
{
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
    }

    private void FixedUpdate() {
        updateFollowCounter -= Time.deltaTime;

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
