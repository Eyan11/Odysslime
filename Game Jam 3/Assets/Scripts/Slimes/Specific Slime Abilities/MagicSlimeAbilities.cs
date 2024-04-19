using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.Rendering.Universal;

//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class MagicSlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private GameObject slimeModel;
    [SerializeField] private Material outline;
    [SerializeField] private float interactionDistance = 6.0f;
    [SerializeField] private float controlObjMoveSpeed = 4.5f;
    [SerializeField] private float minimumControlTime = 5.0f;
    [Header("Radius should be bigger than the slime itself")]
    [SerializeField] private float frontalInteractionRadius = 4.0f;
    private float nextCheckWait = 0.1f;
    private UIManager UIScript;
    private float nextCheckTime;
    private float controlEndMinTime;
    private int movablesMask;
    private SoundManager soundManager;
    private SlimeVitality slimeVitality;
    private SlimePossess slimePossess;
    private SlimeFollowerMovement slimeFollowerMovement;
    private RaycastHit raycastHit;
    private ThirdPersonCam cameraScript;
    private GameObject controlObj;
    private Transform controlObjOrientation;
    private Rigidbody controlObjRigidbody;
    private Movable movable;
    private GameObject priorMovable;
    private Renderer movableRenderer;
    Material[] movableMaterials;
    public bool abilityActive = false;

    private void Awake() {
        // Creates bit mask for pushable objects
        movablesMask = 1 << 9;

        // Retrieves various components
        slimeFollowerMovement = GetComponent<SlimeFollowerMovement>();
        cameraScript = Camera.main.gameObject.GetComponent<ThirdPersonCam>();
        soundManager = FindObjectOfType<SoundManager>();
        slimeVitality = GetComponent<SlimeVitality>();
        slimePossess = GetComponent<SlimePossess>();
        UIScript = FindObjectOfType<UIManager>();

        // Sets the first raycast check time
        nextCheckTime = Time.fixedTime + nextCheckWait;
    }

    private void FixedUpdate() {
        // Checks to see if an object can be interacted with
        InteractionCheck();
    }

    new private void Update() {
        // Runs the normal Update() procedures
        base.Update();

        // Does a keybind check
        SlimePossessReturnKeyCheck();

        // Attempts to move the object
        MoveControlledObject();
    }

    private void SlimePossessReturnKeyCheck() {
        // Attempts to use ability if return to slime king input and ability is active
        if (abilityActive && slimeInput.GetReturnToKingInput()) {
            UseAbility();
        } 
    }

    private void MoveControlledObject() {
        // Doesn't do anything if there is no controlled object
        if (!controlObj) return;

        Vector2 horizontalInput = slimeInput.GetMoveInput(); // Horizontal movement
        float verticalInput = slimeInput.GetMoveBlockVertInput(); // Vertical movement

        Vector3 horizontalMovement = (controlObjOrientation.forward * horizontalInput.y) + (controlObjOrientation.right * horizontalInput.x);

        controlObjRigidbody.velocity = horizontalMovement * controlObjMoveSpeed + Vector3.up * verticalInput * controlObjMoveSpeed;
    }

    private void ClearOutline() {
        // Assume that the 2nd element of materials will always have the shader
        movableMaterials[1] = null;
        movableRenderer.materials = movableMaterials;
        // Sets prior movable to null so the outline could be reapplied to same obj
        priorMovable = null;
    }

    private void InteractionCheck() {
        // Prevents interaction check if there is an object under control
        if (controlObj) {
            if (controlEndMinTime < Time.fixedTime) {
                UIScript.DisplayPrompt("Press Q or Z to stop shifting block!", 0.2f);
            }

            return;
        }

        // Prevents a raycast hit check until enough time passes
        if (nextCheckTime > Time.fixedTime) return;
        nextCheckTime = Time.fixedTime + nextCheckWait;

        // Raycast check on pushable objects
        Physics.SphereCast(transform.position - slimeModel.transform.forward * frontalInteractionRadius, frontalInteractionRadius, slimeModel.transform.forward, out raycastHit, interactionDistance, movablesMask);

        // Checks if theres an object detected; clear out highlight if there isn't;
        if (!raycastHit.collider && priorMovable) {
            ClearOutline();
        } else if (raycastHit.collider && priorMovable != raycastHit.collider.gameObject) {
            // Sets priorMovable
            priorMovable = raycastHit.collider.gameObject;
            // Sets outline to 2nd element for materials
            // Note we get the parent since the Pushable hitbox is what is getting detected
            movableRenderer = priorMovable.transform.parent.GetComponentInChildren<Renderer>();
            movableMaterials = movableRenderer.materials;
            movableMaterials[1] = outline;
            movableRenderer.materials = movableMaterials;
        }

        // Display prompt message for 2/10 a second
        UIScript.DisplayPrompt("Press Q to shift block!", 0.2f);
    }

    public override void UseAbility() {
        if (!abilityActive) {
            // Prevents the usage of the ability if its already in effect
            if (!raycastHit.collider) return;
            // Otherwise, activates the ability and disables keybind slime possession
            abilityActive = true;
            slimePossess.canUsePossessKeybind = false;
            // Sets references
            controlObj = raycastHit.collider.gameObject.transform.parent.gameObject;
            controlObjRigidbody = controlObj.GetComponent<Rigidbody>();

            if (!controlObj || !controlObjRigidbody) {
                Debug.LogError("Unable to find a controlled object's property!");
            }

            // Disables rigidbody stuff to float and not roll
            controlObjRigidbody.freezeRotation = true;
            controlObjRigidbody.useGravity = false;

            // Sets slime's speed to zero
            slimeFollowerMovement.enabled = false;

            // Allows the object to move if it couldn't already
            movable = controlObj.GetComponent<Movable>();
            if (movable) {
                movable.ActiveState();
            }

            // Clears block outline
            ClearOutline();

            // Uses it to change the camera's focus
            cameraScript.SwitchCamera(controlObj);
            // Obtains orientation that the camera generates (if it didn't exist already)
            controlObjOrientation = controlObj.transform.Find("Orientation");

            // Sets control start time
            controlEndMinTime = Time.fixedTime + minimumControlTime;
        }
        else if (controlEndMinTime < Time.fixedTime) { // Prevents player from disengaging for a little
            // Re-enables rigidbody settings
            controlObjRigidbody.freezeRotation = false;
            controlObjRigidbody.useGravity = true;

            // Allows the object to freeze again once done moving
            movable.DeactivatedState();

            // Kills slime
            slimeVitality.enabled = false;
        }
    }

    private void OnDisable() {
        if (priorMovable) {
            ClearOutline();
        }
    }
}
