using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class IceSlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private GameObject iceCubeTemplate;
    [SerializeField] private GameObject orientation;
    [SerializeField] private float interactionDist = 1.2f;
    private RaycastHit raycastHit;
    private int layerMask = 1<<8; // 8 points towards "Ice" layer
    private bool foundIce = false;

    private bool triggered = false;

    private void FixedUpdate() {
        CheckForIceObject();
    }

    private void CheckForIceObject() {
        // Checks if an ice block is in front
        foundIce = Physics.Raycast(transform.position, orientation.transform.forward, out raycastHit, interactionDist, layerMask);

        if (foundIce) { return; }

        // Check if an ice puddle is below
        // TODO: needs own layer
        //foundIce = Physics.Raycast(transform.position, Vector3.down, out raycastHit, interactionDist, layerMask);

        //if (foundIce) { return; }
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
                iceBlock.GrowCube();
            }

            return;
        }
        
        if (!iceCubeTemplate) {
            Debug.LogError("No ice cube template provided!");
            return;
        }

        GameObject iceCube = Instantiate(iceCubeTemplate, transform.position, quaternion.identity);
    }
}
