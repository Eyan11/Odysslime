using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class IceSlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private GameObject iceCubeTemplate;
    private SlimeVitality slimeVitality;
    private SoundManager soundManager;

    private bool triggered = false;

    private void Awake() {
        slimeVitality = GetComponent<SlimeVitality>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    public void GenerateIceCube() {
        if (!iceCubeTemplate) {
            Debug.LogError("No ice cube template provided!");
            return;
        }

        // Creates ice cube at position
        Vector3 icePos = transform.position + new Vector3(0, iceCubeTemplate.transform.localScale.y / 2, 0);
        Instantiate(iceCubeTemplate, icePos, quaternion.identity);
    }
    
    public override void UseAbility()
    {
        // Should only be triggerable once
        if (triggered) {
            return;
        }
        triggered = true;

        GenerateIceCube();

        // Plays freeze sound
        soundManager.PlaySlimeFreeze();

        // Always kills slime
        slimeVitality.enabled = false;
    }
}
