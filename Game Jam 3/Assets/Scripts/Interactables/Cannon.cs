using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cannon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider hitbox;
    [Header("Settings")]
    [SerializeField] private float minBindTime = 5.0f;

    private InputMap inputMap;
    private TextMeshProUGUI textBox;
    private RectTransform UITransform;
    private Transform cameraTransform;
    private FaceObjectYAxis faceObjectYAxis;
    private UIManager UIScript;
    private GameObject UICanvas;
    private PauseMenuManager pauseMenuManager;
    private SlimePossess kingSlimePossess;
    private SlimeInput kingSlimeInput;
    private int numOfSlimesInRange = 0;
    private int numOfSlimelignsInRange = 0;
    private int totalNumOfSlimelings;
    private bool kingSlimeInRange = false;
    private float timeHeld = 0;

    // Retrieves components
    private void Awake()
    {
        if (!hitbox) { // Safety check
            Debug.LogError("Missing hitbox!");
            this.enabled = false;
        }

        // Retrieves a ton of components through built-in functions
        UITransform = GetComponentInChildren<RectTransform>();
        UICanvas = GetComponentInChildren<Canvas>().gameObject;
        cameraTransform = FindObjectOfType<Camera>().transform;
        faceObjectYAxis = GetComponentInChildren<FaceObjectYAxis>();
        textBox = GetComponentInChildren<TextMeshProUGUI>();
        UIScript = FindObjectOfType<UIManager>();
        pauseMenuManager = FindObjectOfType<PauseMenuManager>();
        // Retrieves king slime possession
        GameObject kingSlime = FindObjectOfType<KingMovement>().gameObject;
        kingSlimePossess = kingSlime.GetComponent<SlimePossess>();
        kingSlimeInput = kingSlime.GetComponent<SlimeInput>();

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

        // Only display the prompt if king slime is in radius AND in control
        if (!kingSlimeInRange || !kingSlimePossess.enabled) return;
        // UI Prompt
        int usingController = pauseMenuManager.GetIsUsingKBM() == true ? 0 : 1;
        UIScript.DisplayPrompt("[" + inputMap.Slime.Jump.GetBindingDisplayString(usingController) + 
                               "] to launch!", 0.2f);
        // Add to timer if the proper key is pressed down
        if (kingSlimeInput.GetJumpHeldInput()) {
            timeHeld += Time.deltaTime;
        } else { // Otherwise reset it
            timeHeld = 0;
        }
        // If it's been held down for the mindBindTime, then launch
        if (timeHeld > minBindTime) {
            // SWITCH SCENES HERE EYANANANANANA
            Debug.Log("LAUNCH TIME");
        }
    }

    private bool IsASlime(GameObject obj) {
        return obj.layer == LayerMask.NameToLayer("Slime"); // Slime layer check
    }

    private bool IsASlimeling(GameObject obj) {
        return obj.tag == "Slimeling";
    }

    private bool IsKingSlime(GameObject obj) {
        return obj.tag == "King Slime";
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
