using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> imageList;
    [SerializeField] private CutsceneType cutsceneType = CutsceneType.None;
    [Header("References")]
    [SerializeField] private MainMenuManager mainMenuScript;
    [SerializeField] private AudioClip nextImageSFX;
    [SerializeField] private TMP_Text inputText;
    private SoundManager soundScript;
    //only for jester cutscene
    private PauseMenuManager pauseScript;
    private SaveManager saveScript;
    private InputMap inputMap;
    private bool backInput = false;
    private bool selectInput = false;
    private int imageIndex = 0;

    private void Awake() {
        //create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.UI.Enable();

        //references
        saveScript = GameObject.FindWithTag("Save Manager").GetComponent<SaveManager>();
        soundScript = GameObject.FindWithTag("Sound Manager").GetComponent<SoundManager>();
        if(cutsceneType == CutsceneType.Jester)
            pauseScript = GameObject.FindWithTag("Pause Menu Manager").GetComponent<PauseMenuManager>();

        //start checking for controllers
        StartCoroutine(UpdateInputText());
    }

    private void Update() {
        GetInput();
        CutsceneController();
    }

    private void GetInput() {
        //back input is triggered by back OR pause input
        backInput = (inputMap.UI.CutscenePrevious.triggered || inputMap.UI.Pause.triggered);
        selectInput = inputMap.UI.CutsceneNext.triggered;
    }

    private void CutsceneController() {

        //if select input is triggered
        if(selectInput) {
            //play sound
            soundScript.PlayGlobalSoundEffect(nextImageSFX);
            
            //if there is another cutscene image to watch
            if((imageList.Count - 1) > imageIndex) {

                //enable next cutscene, disable previous cutscene
                imageIndex++;
                imageList[imageIndex].SetActive(true);
                imageList[imageIndex - 1].SetActive(false);
            }
            //if there are no more cutscene images, finish cutscene
            else {
                FinishedCutscene();
            }
        }

        //if back input is triggered
        if(backInput) {
            //play sound
            soundScript.PlayGlobalSoundEffect(nextImageSFX);
            //if not at first cutscene, goto previous cutscene image
            if(imageIndex > 0) {

                //enable previous cutscene, disable current cutscene
                imageIndex--;
                imageList[imageIndex].SetActive(true);
                imageList[imageIndex + 1].SetActive(false);
            }
        }
    }

    //Handles end of cutscene for all cutscenes
    private void FinishedCutscene() {
        
        switch(cutsceneType) {
            
            case CutsceneType.Volcano:
                //tell save manager that we have watched volcano cutscene (only watch once)
                saveScript.FinishedVolcanoCutscene();
                mainMenuScript.OpenLevelSelectMenu();
                gameObject.SetActive(false);
                break;

            case CutsceneType.Jester:
                //trigger event, allows slime input and possesstion again (was disabled for cutscene)
                GameEvents.current.TriggerResumeEvent();
                //enable pause input
                pauseScript.EnableInput();
                gameObject.SetActive(false);
                break;
            case CutsceneType.None:
                break;
        }
    }

    private enum CutsceneType {
        Volcano, Jester, None
    }

    private void OnEnable() {

        //disable all but the first cutscene
        for(int i = 1; i < imageList.Count; i++) {
            imageList[i].SetActive(false);
        }
        //enable first cutscene
        imageList[0].SetActive(true);

        //reset cutscene index
        imageIndex = 0;

        //if in MainMenu scene, disable main menu input
        if(cutsceneType == CutsceneType.Volcano)
            mainMenuScript.OnLoadingScreen();
        //if in island scene, disable slime input and possession
        else if(cutsceneType == CutsceneType.Jester) {
            GameEvents.current.TriggerPauseEvent();
            //disable pause input
            pauseScript.DisableInput();
        }
    }

    //Checks which input is plugges in every 1 second
    private IEnumerator UpdateInputText() {

        Debug.Log("Start Coroutine");

        //if there are any input devices connected
        if(Input.GetJoystickNames().Length != 0) {
            Debug.Log("IF 1");

            //if using Keyboard (empty string is KBM name)
            if (Input.GetJoystickNames()[0] == "") {
                inputText.text = "Press Space to continue";
                Debug.Log("IF 2");
            }
            //if using gamepad
            else {
                inputText.text = "Press A to continue";
                Debug.Log("IF 3");
            }
        }
        //display keyboard by default
        else
            inputText.text = "Press Space to continue";

        //wait 1 seconds before checking input again
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(UpdateInputText());
    }
}
