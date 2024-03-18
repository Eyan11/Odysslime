using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveSlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private float blastRadius = 3.5f;

    private SlimeVitality slimeVitality;
    private void Awake() {
        slimeVitality = GetComponent<SlimeVitality>();
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

        // Kills slime
        slimeVitality.enabled = false;
    }
}
