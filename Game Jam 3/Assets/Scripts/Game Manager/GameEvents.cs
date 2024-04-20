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

    //triggers pause event
    public void PauseEvent() {
        if(onPauseEvent != null)
            onPauseEvent();
    }

    //triggers resume event
    public void ResumeEvent() {
        if(onResumeEvent != null) 
            onResumeEvent();
    }

}
