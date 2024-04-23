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

    //triggers pause event
    public void TriggerPauseEvent() {
        if(onPauseEvent != null)
            onPauseEvent();
    }

    //triggers resume event
    public void TriggerResumeEvent() {
        if(onResumeEvent != null) 
            onResumeEvent();
    }

    //triggers finish island event
    public void TriggerFinishIslandEvent() {
        if(onFinishIslandEvent != null)
            onFinishIslandEvent();
    }

}
