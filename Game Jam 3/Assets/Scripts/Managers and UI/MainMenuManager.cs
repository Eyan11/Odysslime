using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu UI Objects")]
    [SerializeField] private GameObject bgImage;
    [SerializeField] private GameObject titleUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject controlsUI;
    [SerializeField] private GameObject levelSelectUI;
    [SerializeField] private GameObject keyboardImage;
    [SerializeField] private GameObject gamepadImage;

    [Header("Event System First Selected Objects")]
    [SerializeField] private GameObject TitleMenuFirst;
    [SerializeField] private GameObject optionsMenuFirst;
    [SerializeField] private GameObject levelSelectMenuFirst;

    [Header("Level Select References")]
    [SerializeField] private LevelSelectManager levelScript;

    [Header("Cutscene References")]
    [SerializeField] private GameObject volcanoCutscene;

    private CurrentMenu currentMenu;
    private InputMap inputMap;
    private SaveManager saveScript;
    private bool backInput;
    private bool isRunningCoroutine = false;
    private bool isUsingKBM = true;

    private void Awake() {
        //create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.UI.Enable();

        //get reference to save script (persistent)
        saveScript = GameObject.FindWithTag("Save Manager").GetComponent<SaveManager>();

        currentMenu = CurrentMenu.Title;

        //if already seen volcano cutscene, then return to level select instead of title screen
        if(saveScript.SeenVolcanoCutscene() == true)
            OpenLevelSelectMenu();
        //if have NOT scene volcano cutscene, keep title screen displayed and set play button as selected
        else
            EventSystem.current.SetSelectedGameObject(TitleMenuFirst);

        //start checking for controllers
        StartCoroutine(CheckForControllers());

        //lock cursor to game window
        Cursor.lockState = CursorLockMode.Confined;
        //make cursor visible
        Cursor.visible = true;
    }

    private void Update() {
        GetInput();

        //let cutscene manager control game during cutscene
        if(currentMenu != CurrentMenu.Cutscene)
            TitleMenuController();
        
        //if on controls screen, display correct controls image
        if(currentMenu == CurrentMenu.Controls)
            ControlsMenuController();
    }

    private void GetInput() {
        //back input is triggered by back OR pause input
        backInput = (inputMap.UI.Back.triggered || inputMap.UI.Pause.triggered);
    }

    private void TitleMenuController() {
        
        if(backInput && currentMenu != CurrentMenu.Title) {
            OpenTitleMenu();
        }
    }

    private void ControlsMenuController() {

        //if keyboard controls are displayed and NOT using keyboard
        if(keyboardImage.activeInHierarchy == true && !isUsingKBM) {
            //display gamepad controls instead
            keyboardImage.SetActive(false);
            gamepadImage.SetActive(true);
        }
        //if gamepad controls are displayed and using keyboard
        else if(gamepadImage.activeInHierarchy == true && isUsingKBM) {
            //display keyboard controls instead
            keyboardImage.SetActive(true);
            gamepadImage.SetActive(false);
        }
        //if no controls are displayed (which should never happen), display correct controls screen
        else if(gamepadImage.activeInHierarchy == false && keyboardImage.activeInHierarchy == false) {
            
            if(isUsingKBM) {
                keyboardImage.SetActive(true);
                gamepadImage.SetActive(false);
            }
            else {
                keyboardImage.SetActive(false);
                gamepadImage.SetActive(true);
            }
        }
    }


    //--------------------- Opens/Closes Desired Menu ---------------------\\

    private void OpenTitleMenu() {
        CloseCurrentMenu();
        titleUI.SetActive(true);
        bgImage.SetActive(false);
        currentMenu = CurrentMenu.Title;
        EventSystem.current.SetSelectedGameObject(TitleMenuFirst);
    }

    private void OpenOptionsMenu() {
        CloseCurrentMenu();
        optionsUI.SetActive(true);
        currentMenu = CurrentMenu.Options;
        EventSystem.current.SetSelectedGameObject(optionsMenuFirst);
    }

    private void OpenControlsMenu() {
        CloseCurrentMenu();
        controlsUI.SetActive(true);
        currentMenu = CurrentMenu.Controls;
    }

    //public for volcano cutscene to call when finished
    public void OpenLevelSelectMenu() {
        CloseCurrentMenu();
        //distable title UI here instead of CloseCurrentMenu 
        //  because I want it enabled for options and controls still
        titleUI.SetActive(false);

        //if haven't seen volcano cutscene, play it
        if(saveScript.SeenVolcanoCutscene() == false) {
            volcanoCutscene.SetActive(true);
            currentMenu = CurrentMenu.Cutscene;
        }
        //if seen volcano cutscene, go straight to level select
        else {
            levelSelectUI.SetActive(true);
            currentMenu = CurrentMenu.LevelSelect;
            EventSystem.current.SetSelectedGameObject(levelSelectMenuFirst);
        }
    }

    private void CloseCurrentMenu() {
        EventSystem.current.SetSelectedGameObject(null);

        //disables the gameobject of the current UI Menu
        switch(currentMenu) {

            case CurrentMenu.Title:
                bgImage.SetActive(true);
                break;

            case CurrentMenu.LevelSelect:
                levelScript.CloseLevelSelectMenu();
                levelSelectUI.SetActive(false);
                break;

            case CurrentMenu.Options:
                optionsUI.SetActive(false);
                break;

            case CurrentMenu.Controls:
                controlsUI.SetActive(false);
                break;

            case CurrentMenu.Cutscene:
                //cutscenes disable themselves in CutsceneManager script
                break;

        }
    }

    private enum CurrentMenu {
        Title, LevelSelect, Options, Controls, Cutscene
    }

    private void QuitGame() {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    //set by cutscene manager for loading screens so no input is received from this script
    public void OnLoadingScreen() {
        currentMenu = CurrentMenu.Cutscene;
    }

    // ---------------------- Methods for Buttons ---------------------\\

    public void BackButton() {
        StartCoroutine(ButtonCoroutine("OpenTitleMenu"));
    }

    public void OptionsButton() {
        StartCoroutine(ButtonCoroutine("OpenOptionsMenu"));
    }

    public void ControlsButton() {
        StartCoroutine(ButtonCoroutine("OpenControlsMenu"));
    }

    public void LevelSelectButton() {
        StartCoroutine(ButtonCoroutine("OpenLevelSelectMenu"));
    }

    public void QuitButton() {
        StartCoroutine(ButtonCoroutine("QuitGame"));
    }


    // ------------------------ Coroutines -----------------------------\\

    //Stalls button action by 0.2 seconds so animation can finish
    private IEnumerator ButtonCoroutine(string methodName) {

        if(isRunningCoroutine == false) {

            //wait 0.2 seconds so animation can finish and disallow another coroutine
            isRunningCoroutine = true;
            yield return new WaitForSecondsRealtime(0.2f);

            //call method and allow another coroutine
            isRunningCoroutine = false;
            Invoke(methodName, 0f);
        }
    }

    //Checks which input is plugges in every 1 second
    private IEnumerator CheckForControllers() {

        //if there are any input devices connected
        if(Input.GetJoystickNames().Length != 0) {

            //if NOT using keyboard and input name is "" (empty string is KBM name)
            if (!isUsingKBM && Input.GetJoystickNames()[0] == "") {
                //using keyboard
                isUsingKBM = true;
            
            //if using keyboard and input name is NOT "" (empty string is KBM name)
            } else if (isUsingKBM && Input.GetJoystickNames()[0] != "") {    
                //using gamepad     
                isUsingKBM = false;
            }
        }
        else
            //Debug.Log("No Input connected, keyboard controls displayed by default");

        //wait 1 seconds before checking input again
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(CheckForControllers());
    }
}
