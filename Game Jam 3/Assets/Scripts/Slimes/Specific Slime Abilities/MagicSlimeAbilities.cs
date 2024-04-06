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
    private SoundManager soundManager;
    private SlimeFollowerMovement slimeFollowerMovement;
    private RaycastHit raycastHit;
    private ThirdPersonCam cameraScript;
    private bool abilityActive = false;

    private void Awake() {
        // Creates bit mask for pushable objects
        movablesMask = 1 << 9;

        // Retrieves various components
        slimeFollowerMovement = GetComponent<SlimeFollowerMovement>();
        cameraScript = GetComponent<ThirdPersonCam>();
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
    }

    public override void UseAbility() {
        if (!abilityActive) {
            // Prevents the usage of the ability if its already in effect
            if (!raycastHit.collider) return;
            // Otherwise, activates the ability
            abilityActive = true;

            // Disables slime's speed
            slimeFollowerMovement.movementSpeed = 0;

            // Retrieves collider object
            GameObject colliderObj = raycastHit.collider.gameObject;

            cameraScript.SwitchCamera(colliderObj);
        }
    }
}
