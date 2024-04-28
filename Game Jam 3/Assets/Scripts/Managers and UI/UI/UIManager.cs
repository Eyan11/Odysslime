using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header ("HUD References")]
    [SerializeField] private Animator slimelingAnim;
    [SerializeField] private TMP_Text slimelingText;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private int totalSlimelings;

    [Header ("Cutscenes and Loading Screens (Ignore if Not Applicable)")]
    [SerializeField] private GameObject cannonLoadingScreen;
    [SerializeField] private GameObject jesterCutscene;
    [Header ("Ability Overlays")]
    [SerializeField] private GameObject possessOverlay;
    [SerializeField] private GameObject magicOverlay;
    private SaveManager saveScript;
    private Cannon cannonScript;
    private int slimelingsCollected = 0;
    private float promptCountdown = 0;
    private float numberCountdown = 0;
    private int loseSlimelingHash;
    private int gainSlimelingHash;


    private void Awake() {
        //set initial slimeling count
        slimelingText.text = "" + slimelingsCollected + " / " + totalSlimelings;
        
        //get references to parameters and store as ints for 
        loseSlimelingHash = Animator.StringToHash("loseSlimeling");
        gainSlimelingHash = Animator.StringToHash("gainSlimeling");

        saveScript = GameObject.FindWithTag("Save Manager").GetComponent<SaveManager>();
        cannonScript = GameObject.FindWithTag("Cannon").GetComponent<Cannon>();
    }

    private void Update() {
        promptCountdown -= Time.deltaTime;
        numberCountdown -= Time.deltaTime;

        //display no prompt after timer ends (can use any amount of time)
        if(promptCountdown < 0) {
            DisplayPrompt("", 60f);
        }

        // hide countdown after timer ends
        if (numberCountdown < 0) {
            countdownText.text = "";
            numberCountdown = 60.0f;
        }
    }

    public void UpdateSlimelingCount(int changeInSlimes) {

        //update counter and UI for slimeling
        slimelingsCollected += changeInSlimes;
        slimelingText.text = "" + slimelingsCollected + " / " + totalSlimelings;
    
        //prevent null reference errors
        if(slimelingAnim == null)
            return;

        //update UI image
        if(changeInSlimes > 0)
            slimelingAnim.SetTrigger(gainSlimelingHash);
        else
            slimelingAnim.SetTrigger(loseSlimelingHash);
    }

    public void DisplayPrompt(string prompt, float displayTime) {

        //reset counter and display prompt
        promptCountdown = displayTime;
        promptText.text = prompt;
    }

    public void DisplayCountdownNumber(int number, float displayTime) {
        numberCountdown = displayTime;

        // Colors the number depending on how low it is
        if (number > 4) {
            countdownText.color = Color.white;
        } else if (number > 2) {
            countdownText.color = Color.yellow;
        } else {
            countdownText.color = Color.red;
        }

        countdownText.text = "" + number;
    }

    public int GetTotalSlimelings() {
        return totalSlimelings;
    }

    public int GetSlimelings() {
        return slimelingsCollected;
    }
   
    // -------------- Finish Island Event Methods -------------------\\

    private void Start() {
        //needs to be in start to give GameEvents enough time to get set up
        //subscribe to events
        GameEvents.current.onFinishIslandEvent += SaveSlimeCount;
        GameEvents.current.onFinishIslandEvent += StartCannonLoadingScreen;

        //start jester cutscene
        if(jesterCutscene != null) {
            jesterCutscene.gameObject.SetActive(true);
        }
    }

    private void OnDestroy() {
        //unsubscribes from event (avoid null reference when slime dies)
        GameEvents.current.onFinishIslandEvent -= SaveSlimeCount;
        GameEvents.current.onFinishIslandEvent -= StartCannonLoadingScreen;
    }

    //Method is called when canon is used and the island is finished
    private void SaveSlimeCount() {
        if(saveScript != null) {
            //save slime count for current island

            //saves number of slimelings that were put in the cannon, and the total slimelings in level
            saveScript.SaveSlimeCount(cannonScript.GetSlimelingsInRange(), totalSlimelings);
        }
    }

    private void StartCannonLoadingScreen() {
        cannonLoadingScreen.SetActive(true);
    }

    // ------------------- Ability Overlay Methods --------------------\\

    public void SetPossessOverlay(bool enableStatus) {
        possessOverlay.SetActive(enableStatus);
    }

    public void SetMagicOverlay(bool enableStatus) {
        magicOverlay.SetActive(enableStatus);
    }
}
