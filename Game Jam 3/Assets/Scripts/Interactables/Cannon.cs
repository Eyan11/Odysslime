using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Collider hitbox;
    private RectTransform UITransform;
    private Transform cameraTransform;
    private int numOfSlimesInRange = 0;

    // Retrieves components
    private void Awake()
    {
        if (!hitbox) { // Safety check
            Debug.LogError("Missing hitbox!");
            this.enabled = false;
        }

        UITransform = GetComponentInChildren<RectTransform>();
        cameraTransform = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    private void Update()
    {
        // Don't update UI rotation while outta range
        if (numOfSlimesInRange == 0) return;

        Vector2 uiPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 camPos = new Vector2(cameraTransform.position.x, cameraTransform.position.z);

        UITransform.rotation = Quaternion.Euler(0, Vector2.SignedAngle(uiPos, camPos), 0);
    }

    private bool IsASlime(GameObject obj) {
        return obj.layer == 11; // Slime layer check
    }

    private void OnTriggerEnter(Collider collider) {
        if (!IsASlime(collider.gameObject)) return; 

        numOfSlimesInRange++;
    }

    private void OnTriggerExit(Collider collider) {
        if (!IsASlime(collider.gameObject)) return; 

        numOfSlimesInRange--;
    }
}
