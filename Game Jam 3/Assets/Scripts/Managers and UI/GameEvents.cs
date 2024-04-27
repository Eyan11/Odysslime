using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    //global reference to this script
    public static GameEvents current;

    private void Awake() {
        current = this; 
    }

    //list of observers for events
    public event Action onPauseEvent;
    public event Action onResumeEvent;
    public event Action onFinishIslandEvent;


    // ---------------- Trigger Event Methods --------------------\\

    public void TriggerPauseEvent() {
        if(onPauseEvent != null)
            onPauseEvent();
    }

    public void TriggerResumeEvent() {
        if(onResumeEvent != null) 
            onResumeEvent();
    }

    public void TriggerFinishIslandEvent() {
        if(onFinishIslandEvent != null)
            onFinishIslandEvent();
    }
}
