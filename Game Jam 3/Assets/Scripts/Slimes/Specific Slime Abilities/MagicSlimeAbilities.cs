using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class MagicSlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private GameObject slimeModel;
    [SerializeField] private float interactionDistance = 2.0f;
    private float nextCheckWait = 0.1f;
    private float nextCheckTime;
    private int movablesMask;
    private SphereCollider sphereCollider;
    private NavMeshAgent navMeshAgent;
    private SlimeFollowerMovement slimeFollowerMovement;
    private SoundManager soundManager;
    private RaycastHit raycastHit;

    private void Awake() {
        // Creates bit mask for pushable objects
        movablesMask = 1 << 9;

        // Retrieves various components
        sphereCollider = GetComponent<SphereCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        slimeFollowerMovement = GetComponent<SlimeFollowerMovement>();
        soundManager = FindObjectOfType<SoundManager>();

        // Sets the first raycast check time
        nextCheckTime = Time.fixedTime + nextCheckWait;
    }

    private void FixedUpdate() {
        // Prevents a raycast hit check until enough time passes
        if (nextCheckTime > Time.fixedTime) return;
        nextCheckTime = Time.fixedTime + nextCheckWait;

        // Raycast check on pushable objects
        Physics.Raycast(transform.position, slimeModel.transform.forward, out raycastHit, interactionDistance, movablesMask);

        // Checks if the raycast hit anything
        if (raycastHit.collider) {
            Debug.Log("MOVABLE OBJECTTTT");
        }
    }

    public override void UseAbility() {
        GameObject colliderObj = raycastHit.collider.gameObject;

        if (colliderObj) {
            
        }
    }
}
