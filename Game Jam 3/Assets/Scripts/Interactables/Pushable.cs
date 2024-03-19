using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Timeline;

public class Pushable : MonoBehaviour
{
    [Header("Statistics")]
    public int size = 1;
    private Collider internalHitbox;
    private GameObject pushableObj;
    private Collider pushableHitbox;
    private BuildNavMeshSurface buildNavMeshScript;

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

        //Find nav mesh object and get script
        buildNavMeshScript = GameObject.FindWithTag("NavMesh Surface").GetComponent<BuildNavMeshSurface>();
    }

    public void ContinuePush() {
        pushableHitbox.isTrigger = true;
    }

    public void EndPush() {
        pushableHitbox.isTrigger = false;
        
        //update walkable surfaces after moving object
        buildNavMeshScript.UpdateNavMesh();
    }
}
