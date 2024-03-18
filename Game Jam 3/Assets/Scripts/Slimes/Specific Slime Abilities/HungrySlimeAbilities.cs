using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HungrySlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private Transform orientation;
    [SerializeField] private GameObject slimeModel;
    public int slimeSize = 1;
    private int maxSlimeSize = 6;
    private float interactionDistance = 1.2f;
    private int pushablesMask;
    private RaycastHit raycastHit;
    private SphereCollider sphereCollider;

    private void Awake() {
        pushablesMask = 1 << 9;
        pushablesMask |= pushablesMask << 8;

        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnCollisionStay(Collision collision) {
        GameObject obj = collision.gameObject;
        // Checks if its a pushable block
        Pushable pushable = obj.GetComponent<Pushable>();
        if (!pushable) { return; }

        Debug.Log(obj.name + " is pushable!");
    }

    public override void UseAbility() {
        if (slimeSize < maxSlimeSize) {
            // Checks to see if there is an object in front
            // CHANGED TO BLOCKCAST
            if (!Physics.Raycast(transform.position, orientation.forward, out raycastHit, interactionDistance + slimeSize)) {
                return;
            }

            // Checks if its a hungee slime
            GameObject gameObj = raycastHit.collider.gameObject;
            HungrySlimeAbilities hungrySlimeAbilities = gameObj.GetComponent<HungrySlimeAbilities>();

            if (!hungrySlimeAbilities) {
                return;
            }

            // Used for killing slime
            SlimeVitality slimeVitality = gameObj.GetComponent<SlimeVitality>();
            if (!slimeVitality) {
                Debug.LogError("Missing SlimeVitality component in " + gameObj.name);
            }
            slimeVitality.enabled = false;

            // Increases size
            slimeSize++;
            slimeModel.transform.localScale += Vector3.one;
            sphereCollider.radius += 0.5f;
        }
    }
}
