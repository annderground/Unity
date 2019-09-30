using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
///  this class moves the orange ball and detects collisions with the target or data points
/// </summary>
public class MoveBall : MonoBehaviour
{
   // this is the rigidbody of the Orange Ball
    private Rigidbody _rb;

    // the manager collects the data of the whole game
    public Game manager;

    // List to save the Times when ball collides with the Data Points on the Orange Field 
    public List<string> timePoints;
    
    // Variables to control the movement of the Orange ball
    public float forceMultiplier;
    public float jumpStrength;

    //variables needed to set the Ball on the initial position in the next round
    private Transform _startTransform;
    public static Vector3 InitialPosition;
       
    /// <summary>
    /// we save the initial position of the ball, to have it later for the reset of the game
    ///
    /// checking if there is a rigidbody
    /// </summary>
    private void Start()
    {
        _startTransform = GetComponent<Transform>();
        InitialPosition = _startTransform.position;
       
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("No Rigidbody found!");
        } 
    }

    /// <summary>
    ///  in the fixed Update we regulate how the ball is moved 
    /// </summary>
    
    void FixedUpdate()
    {   // as long as Key "W" is down, force is applied in a forward movement
        // when "W" - Key is released, force is immediately stopped for better control of the ball
        if (Input.GetKey(KeyCode.W))
        {
            _rb.AddForce(new Vector3(0,0,1) * forceMultiplier, ForceMode.VelocityChange);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            _rb.velocity=new Vector3(0,0,0);
            _rb.angularVelocity=new Vector3(0,0,0);
        }
      
        
        // as long as Key "S" is down, force is applied in a backward movement
        // when "S" - Key is released, force is immediately stopped for better control of the ball
        if (Input.GetKey(KeyCode.S))
        {
            _rb.AddForce(new Vector3(0,0,-1) * forceMultiplier, ForceMode.VelocityChange);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            _rb.velocity=new Vector3(0,0,0);
            _rb.angularVelocity=new Vector3(0,0,0);
        }
        
        
        // as long as Key "D" is down, force is applied in a rightward movement
        // when "D" - Key is released, force is immediately stopped for better control of the ball
        if (Input.GetKey(KeyCode.D))
        {
            _rb.AddForce(new Vector3(1,0,0) * forceMultiplier, ForceMode.VelocityChange);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            _rb.velocity=new Vector3(0,0,0);
            _rb.angularVelocity=new Vector3(0,0,0);
        }
        
        // as long as Key "A" is down, force is applied in a leftward movement
        // when "A" - Key is released, force is immediately stopped for better control of the ball
        if (Input.GetKey(KeyCode.A))
        {
            _rb.AddForce(new Vector3(-1,0,0) * forceMultiplier, ForceMode.VelocityChange);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            _rb.velocity=new Vector3(0,0,0);
            _rb.angularVelocity=new Vector3(0,0,0);
        }
    }
    
    /// <summary>
    /// the Ball can jump for no apparent reason
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(new Vector3(0,1,0) * jumpStrength, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// this function controls what happens, if the ball collides with either the data point or the target
    /// </summary>
    /// <param name="other"> other is either the collider of the target or a datapoint </param>
    public void OnTriggerEnter(Collider other)
    {
        
        // if the ball hits the target, the information is passed on the the manager
       if (other.CompareTag("targetOrange")) 
       {
         manager.OrangeArrived();
       }
       
       // if the ball hits any data points, the time of collision is saved in a List and the data point is inactivated
       // to save correct times in later rounds, we substract the duration of previous rounds (calculated as Helper2Time in Game)
       else if (other.CompareTag("dataPoint1"))
       {
           timePoints[0] = ((Time.realtimeSinceStartup-Game.Helper2Time).ToString() +" ");
           other.gameObject.SetActive(false);
       }
       else if (other.CompareTag("dataPoint2"))
       {
           timePoints[1] = ((Time.realtimeSinceStartup - Game.Helper2Time).ToString()+" ");
           other.gameObject.SetActive(false);
       }
       else if (other.CompareTag("dataPoint3"))
       {
           timePoints[2] = ((Time.realtimeSinceStartup - Game.Helper2Time).ToString()+" ");
           other.gameObject.SetActive(false);
       }
       else if (other.CompareTag("dataPoint4"))
       {
           timePoints[3] = ((Time.realtimeSinceStartup- Game.Helper2Time).ToString()+" ");
           other.gameObject.SetActive(false);
       }
       else if (other.CompareTag("dataPoint5"))
       {
           timePoints[4] = ((Time.realtimeSinceStartup- Game.Helper2Time).ToString()+" ");
           other.gameObject.SetActive(false);
       }
    }

    /// <summary>
    /// when the ball leaves the target, the manager is informed
    /// </summary>
    /// <param name="other"> other is the collider of the target</param>
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("targetOrange"))
        {
            manager.OrangeLeft();
        }
    }

    
}