using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Scroll Information UI Objects")]
    [SerializeField] private GameObject infernoInfo;
    [SerializeField] private GameObject frostbiteInfo;
    [SerializeField] private GameObject covenInfo;
    [SerializeField] private GameObject engineerInfo;
    [SerializeField] private GameObject oozeInfo;
    [SerializeField] private Animator scrollAnim;
    
    [Header("Scroll Information Text")]
    [SerializeField] private TMP_Text infernoSlimeText;
    [SerializeField] private TMP_Text frostbiteSlimeText;
    [SerializeField] private TMP_Text covenSlimeText;
    [SerializeField] private TMP_Text engineerSlimeText;
    [SerializeField] private TMP_Text oozeSlimeText;

    [Header("Island Buttons (excluding Inferno since always unlocked)")]
    [SerializeField] private Button frostbiteButton;
    [SerializeField] private Button covenButton;
    [SerializeField] private Button engineerButton;
    [SerializeField] private Button oozeButton; 
        
    [Header ("Loading Screen UI Objects")]
    [SerializeField] private GameObject loadingScreenHolder;
    [SerializeField] private GameObject infernoLoadingScreen;
    [SerializeField] private GameObject frostbiteLoadingScreen;
    [SerializeField] private GameObject covenLoadingScreen;
    [SerializeField] private GameObject engineerLoadingScreen;
    [SerializeField] private GameObject oozeLoadingScreen;
    [SerializeField] private GameObject trophyLoadingScreen;
    private CurrentInfo currentInfo;
    private SaveManager saveScript;
    private bool isRunningCoroutine = false;
    private bool isScrollMoving = false;
    private int selectHash;
    private int unselectHash;


    private void Awake() {
        //get references to parameters and store as ints for performace
        selectHash = Animator.StringToHash("select");
        unselectHash = Animator.StringToHash("unselect");

        currentInfo = CurrentInfo.None;

        //Get save script
        saveScript = GameObject.FindWithTag("Save Manager").GetComponent<SaveManager>();

        //TODO: replace 99 with total slime count
        //Gathers save data from SaveManager persistent script
        if(saveScript != null) {

            //Sets slimeling count text for all islands
            infernoSlimeText.text = saveScript.GetIslandSlimesCollected('I') + " / 99";
            frostbiteSlimeText.text = saveScript.GetIslandSlimesCollected('F') + " / 99";
            covenSlimeText.text = saveScript.GetIslandSlimesCollected('C') + " / 99";
            engineerSlimeText.text = saveScript.GetIslandSlimesCollected('E') + " / 99";
            oozeSlimeText.text = saveScript.GetIslandSlimesCollected('O') + " / 99";

            //Sets island lock/unlock state for all islands
            frostbiteButton.interactable = saveScript.IsIslandUnlocked('F');
            covenButton.interactable = saveScript.IsIslandUnlocked('C');
            engineerButton.interactable = saveScript.IsIslandUnlocked('E');
            oozeButton.interactable = saveScript.IsIslandUnlocked('O');
        }
    }


    // ------------ Close Scroll Info for Island Buttons ------------ \\
    private void CloseCurrentScrollInfo() {

        //disables the gameobject of the current scroll information
        switch(currentInfo) {

            case CurrentInfo.Inferno:
                infernoInfo.SetActive(false);
                break;

            case CurrentInfo.Frostbite:
                frostbiteInfo.SetActive(false);
                break;

            case CurrentInfo.Coven:
                covenInfo.SetActive(false);
                break;

            case CurrentInfo.Engineer:
                engineerInfo.SetActive(false);
                break;

            case CurrentInfo.Ooze:
                oozeInfo.SetActive(false);
                break;
        }
    }

    public void CloseLevelSelectMenu() {
        //closes current info on scroll
        CloseCurrentScrollInfo();
        currentInfo = CurrentInfo.None;
        scrollAnim.SetTrigger(unselectHash); 
        isScrollMoving = false;
    }

    private enum CurrentInfo {
        None, Inferno, Frostbite, Coven, Engineer, Ooze
    }

    // ----------- Open Scroll Info for Island Buttons ----------- \\

    public void OpenScrollInfo() {
        //scroll is done animating when called
        isScrollMoving = false;

        //enables the scroll info for the selected island
        switch(currentInfo) {

            case CurrentInfo.Inferno:
                infernoInfo.SetActive(true);
                break;

            case CurrentInfo.Frostbite:
                frostbiteInfo.SetActive(true);
                break;

            case CurrentInfo.Coven:
                covenInfo.SetActive(true);
                break;

            case CurrentInfo.Engineer:
                engineerInfo.SetActive(true);
                break;

            case CurrentInfo.Ooze:
                oozeInfo.SetActive(true);
                break;
        }
    }


    private void TriggerScroll() {

        //if scroll is NOT moving
        if(isScrollMoving == false) {
            
            //play scroll animation
            scrollAnim.SetTrigger(selectHash); 
            isScrollMoving = true;
        }
    }

    // ------------------ Scroll Open Scene Buttons -------------------- \\

    public void VisitIslandButton() {
        StartCoroutine(ButtonCoroutine("StartIslandLoadingSceen", 0.2f));
    }

    private void StartIslandLoadingSceen() {
        //enable loading screen's parent
        loadingScreenHolder.SetActive(true);

        //enable island loading screen 
        switch(currentInfo) {

            case CurrentInfo.Inferno:
                infernoLoadingScreen.SetActive(true);
                break;

            case CurrentInfo.Frostbite:
                frostbiteLoadingScreen.SetActive(true);
                break;

            case CurrentInfo.Coven:
                covenLoadingScreen.SetActive(true);
                break;

            case CurrentInfo.Engineer:
                engineerLoadingScreen.SetActive(true);
                break;

            case CurrentInfo.Ooze:
                oozeLoadingScreen.SetActive(true);
                break;
        }
    }

    public void TrophyRoomButton() {
        StartCoroutine(ButtonCoroutine("StartTrophyLoadingScreen", 0.2f));
    }

    private void StartTrophyLoadingScreen() {
        //enable loading screen's parent
        loadingScreenHolder.SetActive(true);
        //enable trophy loading screen 
        trophyLoadingScreen.SetActive(true);
    }

    // ----------------------- Island Buttons ------------------------- \\

    public void InfernoIslandButton() {
        //if already displaying Inferno info, then ignore
        if(currentInfo == CurrentInfo.Inferno)
            return;
        
        //start scroll anim
        TriggerScroll();

        //hide current info and bring up island info when scroll anim is done
        CloseCurrentScrollInfo();
        currentInfo = CurrentInfo.Inferno;
    }

    public void FrostbiteIslandButton() {
        //if already displaying Frostbite info, then ignore
        if(currentInfo == CurrentInfo.Frostbite)
            return;

        //start scroll anim
        TriggerScroll();

        //hide current info and bring up island info when scroll anim is done
        CloseCurrentScrollInfo();
        currentInfo = CurrentInfo.Frostbite;
    }

    public void CovenIslandButton() {
        //if already displaying Coven info, then ignore
        if(currentInfo == CurrentInfo.Coven)
            return;
        
        //start scroll anim
        TriggerScroll();

        //hide current info and bring up island info when scroll anim is done
        CloseCurrentScrollInfo();
        currentInfo = CurrentInfo.Coven;
    }

    public void EngineerIslandButton() {
        //if already displaying Engineer info, then ignore
        if(currentInfo == CurrentInfo.Engineer)
            return;
        
        //start scroll anim
        TriggerScroll();

        //hide current info and bring up island info when scroll anim is done
        CloseCurrentScrollInfo();
        currentInfo = CurrentInfo.Engineer;
    }

    public void OozeIslandButton() {
        //if already displaying Ooze info, then ignore
        if(currentInfo == CurrentInfo.Ooze)
            return;

        //start scroll anim
        TriggerScroll();

        //hide current info and bring up island info when scroll anim is done
        CloseCurrentScrollInfo();
        currentInfo = CurrentInfo.Ooze;
    }


    // ------------ Coroutines to Wait for Animation to Finish --------- \\

    //Stalls button action so animation can finish
    private IEnumerator ButtonCoroutine(string methodName, float delayTime) {

        if(isRunningCoroutine == false) {

            //wait 0.2 seconds so animation can finish and disallow another coroutine
            isRunningCoroutine = true;
            yield return new WaitForSecondsRealtime(delayTime);

            //call method and allow another coroutine
            isRunningCoroutine = false;
            Invoke(methodName, 0f);
        }
    }
}
