using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> imageList;
    [SerializeField] private CutsceneType cutsceneType = CutsceneType.None;
    [SerializeField] private MainMenuManager mainMenuScript;
    private SaveManager saveScript;
    private InputMap inputMap;
    private bool backInput = false;
    private bool selectInput = false;
    private int imageIndex = 0;

    private void Awake() {
        //create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.UI.Enable();

        saveScript = GameObject.FindWithTag("Save Manager").GetComponent<SaveManager>();
    }

    private void Update() {
        GetInput();
        CutsceneController();
    }

    private void GetInput() {
        //back input is triggered by back OR pause input
        backInput = (inputMap.UI.Back.triggered || inputMap.UI.Pause.triggered);
        selectInput = inputMap.UI.Select.triggered;
    }

    private void CutsceneController() {

        //if select input is triggered
        if(selectInput) {
            
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

                //disable THIS gameobejct
                gameObject.SetActive(false);
                break;

            case CutsceneType.Jester:
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
    }
}
