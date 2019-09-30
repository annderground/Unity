using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this rotates the target cube to make it look nicer
/// </summary>
public class CubeRotation : MonoBehaviour
{
    //rigidbody of target cube
    private Rigidbody _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        // we get the rigidbody of the target cube
        _rb = GetComponent<Rigidbody>();
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // we spin the target cube
        _rb.angularVelocity= new Vector3(0,1,0);
       
    }
}
