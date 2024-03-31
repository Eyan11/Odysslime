using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Acts as a parent for specific slime abilities
abstract public class SlimeAbilities : MonoBehaviour
{
    private SlimeInput inputScript;
    private void Awake() {
        inputScript = GetComponent<SlimeInput>();
    }

    void Update()
    {
        //If ability input is pressed, use ability
        if (inputScript.GetAbilityInput()) {
            UseAbility();
        }
    }

    public abstract void UseAbility();
}
