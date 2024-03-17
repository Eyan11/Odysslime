using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Acts as a parent for specific slime abilities
abstract public class SlimeAbilities : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Checks if the key 'q' is down; if so, trigger ability
        if (Input.GetKey(KeyCode.Q)) {
            UseAbility();
        }
    }

    public abstract void UseAbility();
}
