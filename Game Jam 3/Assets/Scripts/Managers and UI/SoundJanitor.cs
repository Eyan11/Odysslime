using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundJanitor : MonoBehaviour
{
    public AudioSource audioSource;
    public bool isSetUp = false;

    // Update is called once per frame
    void Update()
    {
        // Prevents the component from running if not set up
        if (!isSetUp) return;

        // Error condition if audio source isn't set up
        if (!audioSource) {
            Destroy(this);
        }

        // End condition
        if (!audioSource.isPlaying) {
            Destroy(audioSource);
            Destroy(this);
        }
    }
}
