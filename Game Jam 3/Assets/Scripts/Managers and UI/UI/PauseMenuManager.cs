using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject bgImage;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject controlsUI;
    [SerializeField] private GameObject keyboardImage;
    [SerializeField] private GameObject gamepadImage;

    [Header("Event System First Selected Objects")]
    [SerializeField] private GameObject pauseMenuFirst;
    [SerializeField] private GameObject optionsMenuFirst;

    [Header ("Sounds")]
    [SerializeField] private AudioClip buttonSFX;
    [SerializeField] private AudioClip pauseSFX;
    private SoundManager soundScript;
    private CurrentMenu currentMenu;
    private ThirdPersonCam camScript;
    private InputMap inputMap;
    private bool isPaused = false;
    private bool pauseInput;
    private bool backInput;
    private bool isRunningCoroutine = false;
    private bool isUsingKBM = true;

    private void Awake() {
        //Reference to camera and sound script
        camScript = Camera.main.gameObject.GetComponent<ThirdPersonCam>();
        soundScript = GameObject.FindWithTag("Sound Manager").GetComponent<SoundManager>();

        //create a new Input Map object and enable the UI input
        inputMap = new InputMap();
        inputMap.UI.Enable();

        currentMenu = CurrentMenu.None;

        //start checking for controllers
        StartCoroutine(CheckForControllers());
    }

    private void Update() {
        GetInput();
        PauseMenuController();
        
        //if on controls screen, display correct controls image
        if(currentMenu == CurrentMenu.Controls)
            ControlsMenuController();

        //if controls or options are on screen, allow for player to go back
        if(currentMenu == CurrentMenu.Controls || currentMenu == CurrentMenu.Options)
            BackOnControlsAndOptions();
    }
    
    private void GetInput() {
        pauseInput = inputMap.UI.Pause.triggered;
        backInput = inputMap.UI.Back.triggered;
    }

    private void PauseMenuController() {

        //if not paused and press pause button, pause game
        if(!isPaused && pauseInput)
            PauseGame();

        //if paused and press pause button, resume game
        else if(isPaused && pauseInput)
            ResumeGame();
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

    private void BackOnControlsAndOptions() {

        if(backInput) {
            //play sound
            soundScript.PlayGlobalSoundEffect(pauseSFX);
            //close current menu and go back to pause menu
            OpenPauseMenu();
        }
    }


    //--------------------- Opens/Closes Desired Menu ---------------------\\
    private void OpenPauseMenu() {
        CloseCurrentMenu();
        pauseUI.SetActive(true);
        currentMenu = CurrentMenu.Pause;
        EventSystem.current.SetSelectedGameObject(pauseMenuFirst);
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

    private void CloseCurrentMenu() {
        EventSystem.current.SetSelectedGameObject(null);

        //disables the gameobject of the current UI Menu
        switch(currentMenu) {

            case CurrentMenu.None:
                break;

            case CurrentMenu.Pause:
                pauseUI.SetActive(false);
                break;

            case CurrentMenu.Options:
                optionsUI.SetActive(false);
                break;

            case CurrentMenu.Controls:
                controlsUI.SetActive(false);
                break;
        }
    }

    private enum CurrentMenu {
        None, Pause, Options, Controls
    }

    //--------------------- Pause and Resume UI ---------------------\\
    private void PauseGame() {
        //pause game
        isPaused = true;
        Time.timeScale = 0f;
        soundScript.PlayGlobalSoundEffect(pauseSFX);

        //trigger pause event
        GameEvents.current.TriggerPauseEvent();

        //Set camera sensitivity to 0 when paused
        camScript.UnlockCamera();
        camScript.SetCamSensitivity(0f);

        //enable background and pause menu
        bgImage.SetActive(true);
        OpenPauseMenu();

        //lock cursor to game window
        Cursor.lockState = CursorLockMode.Confined;
        //make cursor visible
        Cursor.visible = true;
    }

    private void ResumeGame() {
        //resume game
        isPaused = false;
        Time.timeScale = 1f;
        soundScript.PlayGlobalSoundEffect(pauseSFX);

        //trigger resume event
        GameEvents.current.TriggerResumeEvent();

        //Set camera sensitivity to normal value when resuming
        camScript.SetCamSensitivity(1f);

        //disable current UI
        bgImage.SetActive(false);
        CloseCurrentMenu();
        currentMenu = CurrentMenu.None;

        //lock cursor to center of game view
        Cursor.lockState = CursorLockMode.Locked;
        //make cursor invisible
        Cursor.visible = false;
    }

    public bool IsPaused() {
        return isPaused;
    }

    // ----------------------- For Game Events (Disable and Enable Input) --------------------\\

    public void DisableInput() {
        //if not in pause menu, allow input to be disabled
        if(currentMenu == CurrentMenu.None)
            inputMap.UI.Disable();
    }

    public void EnableInput() {
        //if not in pause menu, allow input to be disabled
        if(currentMenu == CurrentMenu.None)
            inputMap.UI.Enable();
    }

    // ----------------------- Methods that switch scenes ---------------\\

    private void RestartLevel() {

        //resume game to fix time scale and other problems
        Time.timeScale = 1f;
        //load current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OpenLevelSelectScene() {
        //resume game to fix time scale and other problems
        Time.timeScale = 1f;
        //load level select scene
        SceneManager.LoadScene("MainMenu");
    }

    // ---------------------- Methods for Buttons ---------------------\\

    public void ResumeButton() {
        soundScript.PlayGlobalSoundEffect(buttonSFX);
        StartCoroutine(ButtonCoroutine("ResumeGame"));
    }

    public void OptionsButton() {
        soundScript.PlayGlobalSoundEffect(buttonSFX);
        StartCoroutine(ButtonCoroutine("OpenOptionsMenu"));
    }

    public void ControlsButton() {
        soundScript.PlayGlobalSoundEffect(buttonSFX);
        StartCoroutine(ButtonCoroutine("OpenControlsMenu"));
    }

    public void RestartButton() {
        soundScript.PlayGlobalSoundEffect(buttonSFX);
        StartCoroutine(ButtonCoroutine("RestartLevel"));
    }

    public void LevelSelectButton() {
        soundScript.PlayGlobalSoundEffect(buttonSFX);
        StartCoroutine(ButtonCoroutine("OpenLevelSelectScene"));
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

        //wait 1 seconds before checking input again
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(CheckForControllers());
    }

    // ------------------------ Public Getter Methods -----------------------------\\

    public bool GetIsUsingKBM() {
        return isUsingKBM;
    }
}

