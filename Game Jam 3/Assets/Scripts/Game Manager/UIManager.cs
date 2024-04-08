using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text slimelingText;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private int totalSlimelings;
    private int slimelingsCollected = 0;
    private float promptCountdown = 0;

    private void Awake() {
        slimelingText.text = "Slimelings: " + slimelingsCollected + " / " + totalSlimelings;
    }

    private void Update() {
        promptCountdown -= Time.deltaTime;

        //display no prompt after timer ends (can use any amount of time)
        if(promptCountdown < 0) {
            DisplayPrompt("", 60f);
        }
    }

    public void AddSlimeling() {

        //update counter and UI for slimeling
        slimelingsCollected++;
        slimelingText.text = "Slimelings: " + slimelingsCollected + " / " + totalSlimelings;
    }

    public void DisplayPrompt(string prompt, float displayTime) {

        //reset counter and display prompt
        promptCountdown = displayTime;
        promptText.text = prompt;
    }
   
}
