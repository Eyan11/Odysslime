using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Scroll Information UI Objects")]
    [SerializeField] private GameObject infernoInfo;
    [SerializeField] private GameObject frostbiteInfo;
    [SerializeField] private GameObject covenInfo;
    [SerializeField] private GameObject engineerInfo;
    [SerializeField] private GameObject oozeInfo;
    [SerializeField] private Animator scrollAnim;
    private CurrentInfo currentInfo;
    private bool isRunningCoroutine = false;
    private bool isScrollMoving = false;
    private int selectHash;
    private int unselectHash;

    private void Awake() {
        //get references to parameters and store as ints for performace
        selectHash = Animator.StringToHash("select");
        unselectHash = Animator.StringToHash("unselect");

        currentInfo = CurrentInfo.None;
    }


    // ------------ Closes Current Info on Scroll ------------ \\
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

    // ----------- Method for Island Buttons that Open Info on Scroll ----------- \\

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

    // ----------------------- Methods that Change Scenes -------------------- \\

    private void OpenIslandScene() {
        //opens the scene of the current info variable
        switch(currentInfo) {

            case CurrentInfo.Inferno:
                SceneManager.LoadScene("InfernoIsland");
                break;

            case CurrentInfo.Frostbite:
                SceneManager.LoadScene("FrostbiteIsland");
                break;

            case CurrentInfo.Coven:
                SceneManager.LoadScene("CovenIsland");
                break;

            case CurrentInfo.Engineer:
                SceneManager.LoadScene("EngineerIsland");
                break;

            case CurrentInfo.Ooze:
                SceneManager.LoadScene("Ooze Island");
                break;
        }
    }

    private void OpenTrophyScene() {
        SceneManager.LoadScene("TrophyRoom");
    }


    // ------------------ Method for Island and Scroll Buttons -------------------- \\

    public void VisitIslandButton() {
        StartCoroutine(ButtonCoroutine("OpenIslandScene", 0.2f));
    }

    public void TrophyRoomButton() {
        StartCoroutine(ButtonCoroutine("OpenTrophyScene", 0.2f));
    }

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
