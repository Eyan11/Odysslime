using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingTextUI : MonoBehaviour
{

    [SerializeField] private float changeTextInterval;
    [SerializeField] private TMP_Text loadingText;
    private float changeTextTimer = 0;
    private int textIndex = 0;
    private bool isLoading = true;
    private bool isUsingKBM = true;


    private void Update() {
        if(isLoading)
            TextController();
    }

    private void TextController() {
        changeTextTimer -= Time.unscaledDeltaTime;

        //when timer finishes, update text and reset timer
        if(changeTextTimer < 0) {
            UpdateText();
            changeTextTimer = changeTextInterval;
        }
    }

    private void UpdateText() {

        //if on lest index, go back to first index
        if(textIndex >= 2)
            textIndex = 0;
        //if not on last index, increment textIndex normally
        else
            textIndex++;

        if(textIndex == 0)
            loadingText.text = "Loading .";
        else if(textIndex == 1)
            loadingText.text = "Loading . .";
        else if(textIndex == 2)
            loadingText.text = "Loading . . .";
    }

    public void FinishedLoading() {
        isLoading = false;

        //Check for input device
        CheckForControllers();

        //displays text depending on KBM or gamepad input
        if(isUsingKBM)
            loadingText.text = "Press Space to play";
        else
            loadingText.text = "Press A to play";
    }


    //Checks which input is plugged in
    private void CheckForControllers() {

        //if there are any input devices connected
        if(Input.GetJoystickNames().Length != 0) {

            //if NOT using keyboard and input name is "" (empty string is KBM name), switch to KBM
            if (!isUsingKBM && Input.GetJoystickNames()[0] == "") 
                isUsingKBM = true;
            
            //if using keyboard and input name is NOT "" (empty string is KBM name), switch to gamepad
            else if (isUsingKBM && Input.GetJoystickNames()[0] != "") 
                isUsingKBM = false;
            
        }
    }
}
