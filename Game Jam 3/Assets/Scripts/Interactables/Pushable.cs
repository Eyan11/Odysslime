using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    [Header("Statistics")]
    public int size = 1;

    private new Rigidbody rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        bool isSleeping = rigidbody.IsSleeping();

        // Prevents the object from being moved unless by a slime
        if (isSleeping) {
            rigidbody.isKinematic = true;
        }
    }
}
