using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject bgImage;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject controlsUI;
    private CurrentMenu currentMenu;
    private ThirdPersonCam camScript;
    private InputMap inputMap;
    private bool isPaused = false;
    private bool pauseInput;
    private bool backInput;
    private bool selectInput;

    private void Awake() {

        //Reference to camera script
        camScript = Camera.main.gameObject.GetComponent<ThirdPersonCam>();

        //create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.UI.Enable();

        currentMenu = CurrentMenu.None;
    }

    private void Update() {
        GetInput();
        MenuController();
    }

    private void GetInput() {
        
        pauseInput = inputMap.UI.Pause.triggered;
        backInput = inputMap.UI.Back.triggered;
        selectInput = inputMap.UI.Select.triggered;
    }

    //used to navigate the pause menu's
    private void MenuController() {

        //if not paused and press pause button, pause game
        if(!isPaused && pauseInput)
            PauseGame();

        //if paused and press pause button, resume game
        else if(isPaused && pauseInput)
            ResumeGame();

        /*  TESTING DIFFERENT MENUS
        if(isPaused && Input.GetKeyDown(KeyCode.O))
            OpenOptionsMenu();
        else if(isPaused && Input.GetKeyDown(KeyCode.P))
            OpenPauseMenu();
        else if(isPaused && Input.GetKeyDown(KeyCode.C))
            OpenControlsMenu();
        */
    }


    //--------------------- Opens/Closes Desired Menu ---------------------\\
    private void OpenPauseMenu() {
        CloseCurrentMenu();
        pauseUI.SetActive(true);
        currentMenu = CurrentMenu.Pause;
    }

    private void OpenOptionsMenu() {
        CloseCurrentMenu();
        optionsUI.SetActive(true);
        currentMenu = CurrentMenu.Options;
    }

    private void OpenControlsMenu() {
        CloseCurrentMenu();
        controlsUI.SetActive(true);
        currentMenu = CurrentMenu.Controls;
    }

    private void CloseCurrentMenu() {
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

        //trigger pause event
        GameEvents.current.PauseEvent();

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

        //trigger resume event
        GameEvents.current.ResumeEvent();

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
}
