using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private int totalDrillParts;
    [SerializeField] private string endSceneName;
    private int drillPartsCollected = 0;
    [SerializeField] private TMP_Text drillText;

    private void Awake() {
        drillText.text = "Drill Parts: " + drillPartsCollected + " / " + totalDrillParts;
    }

    public void AddDrillPart() {

        //update counter and UI for drill parts
        drillPartsCollected++;
        drillText.text = "Drill Parts: " + drillPartsCollected + " / " + totalDrillParts;

        //if collected all drill parts
        if(drillPartsCollected >= totalDrillParts) {

            //win game, load end screen, make sure string is correct
            SceneManager.LoadScene(endSceneName);
        }

    }


}
