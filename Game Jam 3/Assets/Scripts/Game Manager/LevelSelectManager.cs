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
    [SerializeField] private GameObject OozeInfo;
    private CurrentInfo currentInfo;
    private bool isRunningCoroutine = false;


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
                OozeInfo.SetActive(false);
                break;
        }
    }

    private enum CurrentInfo {
        Inferno, Frostbite, Coven, Engineer, Ooze
    }

    // ----------- Methods for Island Buttons that Open Info on Scroll ----------- \\

    private void DisplayInfernoInfo() {
        currentInfo = CurrentInfo.Inferno;
        infernoInfo.SetActive(true);
    }

    private void DisplayFrostbiteInfo() {
        currentInfo = CurrentInfo.Frostbite;
        frostbiteInfo.SetActive(true);
    }

    private void DisplayCovenInfo() {
        currentInfo = CurrentInfo.Coven;
        covenInfo.SetActive(true);
    }

    private void DisplayEngineerInfo() {
        currentInfo = CurrentInfo.Engineer;
        engineerInfo.SetActive(true);
    }

    private void DisplayOozeInfo() {
        currentInfo = CurrentInfo.Ooze;
        OozeInfo.SetActive(true);
    }

    // ---------- Methods for Scroll Buttons that Change Scenes --------- \\

    private void OpenInfernoScene() {

    }

    private void OpenFrostbiteScene() {
        
    }

    private void OpenCovenScene() {
        
    }

    private void OpenEngineerScene() {
        
    }

    private void OpenOozeScene() {
        
    }

    private void OpenTrophyScene() {
        
    }

    // ------------------ Method for Buttons -------------------- \\

    public void VisitIslandButton() {
        StartCoroutine(ButtonCoroutine("DisplayInfernoInfo", 3f));
    }

    public void TrophyRoomButton() {
        StartCoroutine(ButtonCoroutine("DisplayInfernoInfo", 3f));
    }

    public void InfernoIslandButton() {
        //if already displaying Inferno info, then ignore
        if(currentInfo == CurrentInfo.Inferno)
            return;
        
        CloseCurrentScrollInfo();
        StartCoroutine(ButtonCoroutine("DisplayInfernoInfo", 3f));
    }

    public void FrostbiteIslandButton() {
        //if already displaying Frostbite info, then ignore
        if(currentInfo == CurrentInfo.Frostbite)
            return;

        CloseCurrentScrollInfo();
        StartCoroutine(ButtonCoroutine("DisplayFrostbiteInfo", 3f));
    }

    public void CovenIslandButton() {
        //if already displaying Coven info, then ignore
        if(currentInfo == CurrentInfo.Coven)
            return;
        
        CloseCurrentScrollInfo();
        StartCoroutine(ButtonCoroutine("DisplayCovenInfo", 3f));
    }

    public void EngineerIslandButton() {
        //if already displaying Engineer info, then ignore
        if(currentInfo == CurrentInfo.Engineer)
            return;
        
        CloseCurrentScrollInfo();
        StartCoroutine(ButtonCoroutine("DisplayEngineerInfo", 3f));
    }

    public void OozeIslandButton() {
        //if already displaying Ooze info, then ignore
        if(currentInfo == CurrentInfo.Ooze)
            return;

        CloseCurrentScrollInfo();
        StartCoroutine(ButtonCoroutine("DisplayOozeInfo", 3f));
    }

    // ------------ Coroutines to Wait for Animation to Finish --------- \\

    //Stalls button action by 0.2 seconds so animation can finish
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
