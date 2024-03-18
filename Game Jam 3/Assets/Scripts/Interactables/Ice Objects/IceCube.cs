using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class IceCube : Pushable
{
    [Header("Settings")]
    // Represents the visualization
    [SerializeField] private GameObject iceModel;
    public int sizeLimit = 6;

    private BoxCollider boxCollider;

    private void Start() {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Grows ice cube if below size limit
    public void GrowCube()
    {
        if (size < sizeLimit) {
            size++;

            // Increases collider and model size
            boxCollider.size += new Vector3(1, 1, 1);
            gameObject.transform.position += new Vector3(0, 0.5f, 0);
            iceModel.transform.localScale += new Vector3(1, 1, 1);
        }
    }
}
