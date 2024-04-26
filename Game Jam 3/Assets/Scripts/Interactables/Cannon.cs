using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Collider hitbox;
    private InputMap inputMap;
    private TextMeshProUGUI textBox;
    private RectTransform UITransform;
    private Transform cameraTransform;
    private FaceObjectYAxis faceObjectYAxis;
    private UIManager UIScript;
    private GameObject UICanvas;
    private int numOfSlimesInRange = 0;
    private int numOfSlimelignsInRange = 0;
    private int totalNumOfSlimelings;
    private bool kingSlimeInRange = false;

    // Retrieves components
    private void Awake()
    {
        if (!hitbox) { // Safety check
            Debug.LogError("Missing hitbox!");
            this.enabled = false;
        }

        UITransform = GetComponentInChildren<RectTransform>();
        UICanvas = GetComponentInChildren<Canvas>().gameObject;
        cameraTransform = FindObjectOfType<Camera>().transform;
        faceObjectYAxis = GetComponentInChildren<FaceObjectYAxis>();
        textBox = GetComponentInChildren<TextMeshProUGUI>();
        UIScript = FindObjectOfType<UIManager>();

        // Creates inputmap to retrieve button trigger name
        inputMap = new InputMap();

        // Retrieves slime total
        totalNumOfSlimelings = UIScript.GetTotalSlimelings();
    }

    private void Update() {
        // Only update if theres slimes within range
        if (numOfSlimesInRange == 0) return;

        // World text
        textBox.text = numOfSlimelignsInRange + "/" + totalNumOfSlimelings;
        if (numOfSlimelignsInRange == totalNumOfSlimelings) {
            textBox.color = Color.green;
        } else {
            textBox.color = Color.yellow;
        }

        // Only display the prompt if king slime is in radius
        if (!kingSlimeInRange) return;
        // UI Prompt
        UIScript.DisplayPrompt(inputMap.Slime.Jump.GetBindingDisplayString() + 
                               " to launch!", 0.2f);
    }

    private bool IsASlime(GameObject obj) {
        return obj.layer == 11; // Slime layer check
    }

    private bool IsASlimeling(GameObject obj) {
        return obj.tag == "Slimeling";
    }

    private bool IsKingSlime(GameObject obj) {
        return obj.tag == "Slime King";
    }

    private void OnTriggerEnter(Collider collider) {
        GameObject colliderObj = collider.gameObject;
        if (!IsASlime(colliderObj)) return; 

        numOfSlimesInRange++;
        if (IsASlimeling(colliderObj)) {
            numOfSlimelignsInRange++;
        } else if (IsKingSlime(colliderObj)) {
            kingSlimeInRange = true;
        }

        // Enables UI rotation
        if (faceObjectYAxis.enabled) return;
        faceObjectYAxis.enabled = true;
        UICanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider collider) {
        GameObject colliderObj = collider.gameObject;
        if (!IsASlime(colliderObj)) return; 

        numOfSlimesInRange--;
        if (IsASlimeling(colliderObj)) {
            numOfSlimelignsInRange--;
        } else if (IsKingSlime(colliderObj)) {
            kingSlimeInRange = false;
        }

        // Disables UI rotation if no more slimes
        if (!faceObjectYAxis.enabled || numOfSlimesInRange > 0) return;
        faceObjectYAxis.enabled = false;
        UICanvas.SetActive(false);
    }
}
