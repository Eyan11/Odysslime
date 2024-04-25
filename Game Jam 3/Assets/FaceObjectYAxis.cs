using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObjectYAxis : MonoBehaviour
{
    [SerializeField] private Transform objectToLookAt; 
    // Start is called before the first frame update
    void Awake()
    {
        if (!objectToLookAt) {
            // Defaults to cam
            objectToLookAt = FindObjectOfType<Camera>().transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 objectPosYModded = new Vector3(objectToLookAt.position.x,
        transform.position.y,
        objectToLookAt.position.z);

        transform.LookAt(objectToLookAt);
    }
}
