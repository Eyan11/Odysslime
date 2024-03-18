using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class IceSlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private GameObject iceCubeTemplate;
    [SerializeField] private GameObject icePuddleTemplate;
    [SerializeField] private GameObject orientation;
    [SerializeField] private float interactionDistance = 1.2f;
    private RaycastHit raycastHit;
    private SlimeVitality slimeVitality;
    private int iceLayerMask = 1<<8; // 8 points towards "Ice" layer
    private int waterLayerMask = 1<<4; // 4 points towards "Water" layer
    private bool foundIce = false;

    private bool triggered = false;

    private void Awake() {
        slimeVitality = GetComponent<SlimeVitality>();
    }
    
    private void FixedUpdate() {
        CheckForIceObject();
    }

    private void CheckForIceObject() {
        // Checks if an ice block is in front
        foundIce = Physics.Raycast(transform.position, orientation.transform.forward, out raycastHit, interactionDistance, iceLayerMask);

        if (foundIce) { return; }

        // Check if an ice puddle is below
        foundIce = Physics.Raycast(transform.position, Vector3.down, out raycastHit, interactionDistance, iceLayerMask);
    }

    public void GenerateIcePuddle() {
        RaycastHit groundRaycastHit;

        Physics.Raycast(transform.position, Vector3.down, out groundRaycastHit, Mathf.Infinity, waterLayerMask);

        if (!icePuddleTemplate) {
            Debug.LogError("No ice puddle template provided!");
            return;
        }

        Vector3 spawnPoint = groundRaycastHit.point + new Vector3(0, transform.localScale.y * 0.5f, 0);

        // Creates ice cube at position
        Instantiate(icePuddleTemplate, groundRaycastHit.point, quaternion.identity);
    }

    public void GenerateIceCube() {
        if (!iceCubeTemplate) {
            Debug.LogError("No ice cube template provided!");
            return;
        }

        // Creates ice cube at position
        Instantiate(iceCubeTemplate, transform.position, quaternion.identity);
    }
    
    public override void UseAbility()
    {
        // Should only be triggerable once
        if (triggered) {
            return;
        }
        triggered = true;

        if (foundIce) {
            GameObject iceObj = raycastHit.collider.gameObject;

            // Ice cube growth
            IceCube iceBlock = iceObj.GetComponent<IceCube>();
            if (iceBlock) {
                // Grows club
                iceBlock.GrowCube();
                
            }

            // Creates puddle if standing on an existing puddle
            IcePlatform icePuddle = iceObj.GetComponent<IcePlatform>();
            if (icePuddle) {
                GenerateIcePuddle();
            }
        }
        else {
            GenerateIceCube();
        }

        // Always kills slime
        slimeVitality.enabled = false;
    }
}
