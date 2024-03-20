using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenHandler : MonoBehaviour
{
    [SerializeField] private Canvas titleScreen;
    [SerializeField] private Canvas instructionScreen;
    [SerializeField] private String worldScene;
    private bool buttonIsDown = false;
    private int state = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !buttonIsDown) {
            buttonIsDown = true;

            state++;
        }

        if (!Input.anyKey && buttonIsDown) {
            buttonIsDown = false;
        }

        // Instruction screen
        if (state == 1) {
            titleScreen.enabled = false;
            instructionScreen.enabled = true;
        }

        // Go to game
        if (state >= 2) {
            SceneManager.LoadScene(worldScene);
        }
    }
}
