using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed = 1f;
    public float walkSpeed;
    public float runSpeed;


    public float groundDrag;

    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public bool readyToJump = true;
    
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    public bool isGrounded;
    

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    public movementState state;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    public enum movementState
    {
        walking,
        running,
        jump,
    }
    // Start is called before the first frame update
    
    private void StateHandler()
    {
        // mode sprinting 
        if(isGrounded && Input.GetKey(sprintKey))
        {
            state = movementState.running;
            moveSpeed = runSpeed;
        }
        else if (isGrounded)
        {
            state = movementState.walking;
            moveSpeed = walkSpeed;
        }

        else
        {
            state =movementState.jump;
        }
    }
    private void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // check for press 
        if(Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            Debug.Log("Jump");
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }
    // Update is called once per frame
    private void Update()
    {
        myInput();
        speedControl();
        StateHandler();
        // ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);
        // handles drag
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
            movePlayer();
    }

    private void movePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        // on ground
        if(isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
               
        
        // while in air
        else if(!isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //limits speed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
