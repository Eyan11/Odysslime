using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonKingSlimeMovement : SlimeMovement
{
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Awake()
    {
        //grab rigidbody reference and make sure body doesn't fall over
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
}
