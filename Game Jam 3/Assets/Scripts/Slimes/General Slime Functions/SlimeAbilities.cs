using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Acts as a parent for specific slime abilities
abstract public class SlimeAbilities : MonoBehaviour
{
    private SlimeInput slimeInput;

    private void Start() {
        slimeInput = GetComponent<SlimeInput>();
    }
    private void Update()
    {
        //If ability input is pressed, use ability
        if (slimeInput.GetAbilityInput()) {
            UseAbility();
        }
    }

    public abstract void UseAbility();
}
