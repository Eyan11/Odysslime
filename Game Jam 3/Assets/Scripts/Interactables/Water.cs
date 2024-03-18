using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Lethal
{
    protected override void OnTriggerEnter(Collider collider) {
        // Ice slime check
        GameObject gameObj = collider.gameObject;
        IceSlimeAbilities iceSlimeAbilities = gameObj.GetComponent<IceSlimeAbilities>();

        if (iceSlimeAbilities) {
            // Generates a puddle on death on water
            iceSlimeAbilities.GenerateIcePuddle();
        }

        base.OnTriggerEnter(collider);
    }
}
