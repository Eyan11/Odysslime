using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Collider hitbox;
    private TextMeshProUGUI textBox;
    private RectTransform UITransform;
    private Transform cameraTransform;
    private FaceObjectYAxis faceObjectYAxis;
    private UIManager UIScript;
    private GameObject UICanvas;
    private int numOfSlimesInRange = 0;
    private int numOfSlimelignsInRange = 0;
    private int totalNumOfSlimelings;

    // Retrieves components
    private void Awake()
    {
        if (!hitbox) { // Safety check
            Debug.LogError("Missing hitbox!");
            this.enabled = false;
        }

        UITransform = GetComponentInChildren<RectTransform>();

        cameraTransform = FindObjectOfType<Camera>().transform;
        faceObjectYAxis = GetComponentInChildren<FaceObjectYAxis>();
        textBox = GetComponentInChildren<TextMeshProUGUI>();
        UIScript = FindObjectOfType<UIManager>();

        // Retrieves slime total
        totalNumOfSlimelings = UIScript.GetTotalSlimelings();
    }

    private void Update() {
        // Only update if theres slimes within range
        if (numOfSlimesInRange == 0) return;

        if (numOfSlimelignsInRange == totalNumOfSlimelings) {
            textBox.text = "You got all the slimelings!";
        } else {
            textBox.text = "There are still " + (totalNumOfSlimelings - numOfSlimelignsInRange) + " undiscovered slimelings!";
        }
    }

    private bool IsASlime(GameObject obj) {
        return obj.layer == 11; // Slime layer check
    }

    private bool IsASlimeling(GameObject obj) {
        return obj.tag == "Slimeling";
    }

    private void OnTriggerEnter(Collider collider) {
        if (!IsASlime(collider.gameObject)) return; 

        numOfSlimesInRange++;
        if (IsASlimeling(collider.gameObject)) {
            numOfSlimelignsInRange++;
        }

        // Enables UI rotation
        if (faceObjectYAxis.enabled) return;
        faceObjectYAxis.enabled = true;
        UICanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider collider) {
        if (!IsASlime(collider.gameObject)) return; 

        numOfSlimesInRange--;
        if (IsASlimeling(collider.gameObject)) {
            numOfSlimelignsInRange--;
        }

        // Disables UI rotation if no more slimes
        if (!faceObjectYAxis.enabled || numOfSlimesInRange > 0) return;
        faceObjectYAxis.enabled = false;
        UICanvas.SetActive(false);
    }
}
