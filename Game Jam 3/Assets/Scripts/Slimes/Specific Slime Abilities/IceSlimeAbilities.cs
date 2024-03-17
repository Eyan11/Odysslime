using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class IceSlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private GameObject iceCubeTemplate;
    [SerializeField] private float interactionDist = 1.2f;
    private RaycastHit raycastHit;
    private int layerMask = 1<<8; // 8 points towards "Ice" layer
    private bool foundIce = false;

    private bool triggered = false;

    private void FixedUpdate() {
        // Checks if an ice block is in front
        foundIce = Physics.Raycast(transform.position, transform.forward, out raycastHit, interactionDist, layerMask);

        if (foundIce) {
            Debug.Log("Ice cube!!!!!!");
        }

        // Check if an ice puddle is below
    }
    
    public override void UseAbility()
    {
        // Should only be triggerable once
        if (triggered) {
            return;
        }
        triggered = true;

        if (foundIce) {
            Debug.Log("Snow zone time");

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
