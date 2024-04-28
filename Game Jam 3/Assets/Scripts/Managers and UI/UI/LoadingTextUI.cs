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


    private void Update() {
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
}
