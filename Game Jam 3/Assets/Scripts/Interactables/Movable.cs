using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Timeline;

public class Movable : MonoBehaviour
{
    [Header("Statistics")]
    private Collider internalHitbox;
    private GameObject pushableObj;
    private Collider pushableHitbox;

    private void Awake() {
        internalHitbox = GetComponent<Collider>();
        if (!internalHitbox) {
            Debug.LogError(gameObject.name + " is missing a \"Rigidbody\"!");
            return;
        }

        pushableObj = transform.Find("Strength Hitbox").gameObject;
        if (!pushableObj) {
            Debug.LogError(gameObject.name + " is missing a \"Strength Hitbox\"!");
            return;
        }

        pushableHitbox = pushableObj.GetComponent<Collider>();
        if (!pushableHitbox) {
            Debug.LogError("\"Strength Hitbox\" is missing a BoxCollider!");
        }

        Physics.IgnoreCollision(internalHitbox, pushableHitbox, true);
    }

    public void ContinuePush() {
        pushableHitbox.isTrigger = true;
    }

    public void EndPush() {
        pushableHitbox.isTrigger = false;
    }
}
