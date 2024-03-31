using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Acts as a parent for specific slime abilities
abstract public class SlimeAbilities : MonoBehaviour
{

    void Update()
    {
        //If ability input is pressed, use ability
        if (Input.GetKeyDown(KeyCode.Q)) {
            UseAbility();
        }
    }

    public abstract void UseAbility();
}
