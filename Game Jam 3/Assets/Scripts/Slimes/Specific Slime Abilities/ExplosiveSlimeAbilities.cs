using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveSlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private float blastRadius = 3.5f;
    private SlimeVitality slimeVitality;
    private SoundManager soundManager;

    private void Awake() {
        slimeVitality = GetComponent<SlimeVitality>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }
    public override void UseAbility() {
        // Gets all objects within blast radius
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (var obj in objectsInRange) {
            // Kills slimes in range (except king slime cuz they cool)
            SlimeVitality objVitality = obj.GetComponent<SlimeVitality>();
            if (objVitality) {
                objVitality.enabled = false;

                continue;
            }

            // Destroys explodable objects
            Explodable objExplodable = obj.GetComponent<Explodable>();
            if (objExplodable) {
                Destroy(obj.gameObject);

                continue;
            }
        }

        // Plays kaboomy
        soundManager.PlayExplosion();

        // Kills slime
        slimeVitality.enabled = false;
    }
}
