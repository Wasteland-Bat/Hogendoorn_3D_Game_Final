using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public NewPlayerMovement PM;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Detection")]
    public float detectLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private void Update()
    {
        wallCheck();
        stateMachine();
        if (climbing)
               climbingMovement();

    }

    private void stateMachine() 
    {
        // state 1 - climbing
        if(wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing && climbTimer > 0) 

                // turns climbing to true
                startClimbing();

            //timer
            if (climbTimer > 0) 
            climbTimer -= Time.deltaTime;

            if (climbTimer < 0)
            stopClimbing();
        }
        // sate 3 - None
        else
        {
            if(climbing) stopClimbing() ;
        }
    }
    private void wallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if (PM.isGrounded) 
        {
            climbTimer = maxClimbTime;
        }

        //print(wallFront + " " + "WallCheck()");
    }
    private void climbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
             Debug.Log("Going through");
    }

    private void startClimbing()
    {
        climbing = true;
       // PM.gravity = 0;
        Debug.Log("StartClimbing() called  Climbing = true");
    }
    private void stopClimbing()
    {
        climbing = false;
       // PM.gravity = -25.81f;
    }
}
