using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //this is the component that plays the sounds
    [SerializeField] private AudioSource source;

    //this is the individual sound effect you are playing
    //copy and paste this for each sound you want to play
    [SerializeField] private AudioClip soundExample;


    //deletes all other sound manager objects so that there is only 1 in scene
    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Sound Manager");

        //only keep the first sound manager instantiated in scene
        if(objs.Length > 1)
            Destroy(this.gameObject);

        //stays from scene to scene
        DontDestroyOnLoad(this.gameObject);
    }

    //make a method exactly like this with each sound
    public void SoundExample() {
        //second argument is priority level of sound (0-1)
        source.PlayOneShot(soundExample, 0.6f);
    }

    //if you want background music that will always play, 
    //make another audio source component and drag music clip into there and enable loop



    //add this to the script that you call the public sound method from
    /*
    private SoundManager soundScript;
    private void Awake() {
        soundScript = GameObject.FindWithTag("Sound Manager").GetComponent<SoundManager>();
    }

    //when sound should be played put this:
    soundScript.SoundExample();
    */

}
