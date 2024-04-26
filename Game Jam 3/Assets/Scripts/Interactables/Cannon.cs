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

        textBox.text = numOfSlimelignsInRange + "/" + totalNumOfSlimelings;
        if (numOfSlimelignsInRange == totalNumOfSlimelings) {
            textBox.color = Color.green;
        } else {
            textBox.color = Color.yellow;
        }

        // TEMPORARILY GETTING RETURNTOKING KEYBINDS
        UIScript.DisplayPrompt(inputMap.Slime.ReturnToKing.GetBindingDisplayString() + 
                               " to launch!", 0.2f);
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
