using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungrySlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    public int slimeSize = 1;
    private float interactionDistance = 1.2f;
    private int pushablesMask;

    private void Awake() {
        pushablesMask = 1 << 9;
        pushablesMask |= pushablesMask << 8;
    }
    
    private void FixedUpdate() {
        
    }
    public override void UseAbility() {

    }
}
