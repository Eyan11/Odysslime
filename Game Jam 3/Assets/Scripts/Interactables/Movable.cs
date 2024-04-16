using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Timeline;

public class Movable : MonoBehaviour
{
    private Collider internalHitbox;
    private GameObject pushableObj;
    private Collider pushableHitbox;
    private Rigidbody rb;
    private bool isHavingInteractions = false;

    private void Awake() {
        // Hitbox collision calculations
        internalHitbox = GetComponent<Collider>();
        if (!internalHitbox) {
            Debug.LogError(gameObject.name + " is missing a \"Rigidbody\"!");
            return;
        }

        pushableObj = transform.Find("Pushable Hitbox").gameObject;
        if (!pushableObj) {
            Debug.LogError(gameObject.name + " is missing a \"Pushable Hitbox\"!");
            return;
        }

        pushableHitbox = pushableObj.GetComponent<Collider>();
        if (!pushableHitbox) {
            Debug.LogError("\"Pushable Hitbox\" is missing a BoxCollider!");
        }

        Physics.IgnoreCollision(internalHitbox, pushableHitbox, true);

        // Rigidbody retrieval
        rb = GetComponent<Rigidbody>();
    }

    public void ActiveState() {
        isHavingInteractions = true;
        rb.isKinematic = false;
    }

    public void DeactivatedState() {
        isHavingInteractions = false;
    }

    private void Update() {
        if (rb.IsSleeping() && !isHavingInteractions) {
            rb.isKinematic = true;
        }
    }
}
