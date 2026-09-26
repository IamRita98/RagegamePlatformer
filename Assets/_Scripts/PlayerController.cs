using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Left/Right Movement")]
    [SerializeField] private float moveSpeed;
    [Header("Jump & Gravity")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float airControl = .2f;
    [SerializeField] private float risingGravity = 4;
    [SerializeField] private float fallingGravity = 7;
    [SerializeField] private float lingerAtApexGravity = .7f;
    [SerializeField] private float lingeringAirTime; //This is not 1 to 1 of value to seconds, so play around w/ it
    private float defaultGravity = 1;
    [SerializeField] float gravityScaling;

    // Double jump
    [SerializeField] private int maxJumps = 1;
    private int jumpsRemaining;

    // Wall jump
    [SerializeField] private float wallJumpForce = 8f;
    [SerializeField] private float wallJumpHorizontalForce = 8f;
    [SerializeField] private float wallCheckDistance = 0.6f;

    

    Rigidbody rb;
    float horizontalMovement;

    

    bool isGrounded;
    bool isTouchingWall;
    int wallDirection;

    [Header("Ground Check")]
    public Transform groundCollPos;
    [SerializeField] private Vector3 groundCheckBoxSize = new Vector3(.3f, .06f, .01f);
    List<Collider> objectsUnderFeet = new List<Collider>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpsRemaining = maxJumps;
    }

    private void FixedUpdate()
    {
        Gravity();
        MovementPhysics();

        objectsUnderFeet = Physics.OverlapBox(
            groundCollPos.position,
            groundCheckBoxSize,
            Quaternion.identity,
            LayerMask.GetMask("Ground")
        ).ToList();

        CheckForWall();
    }

    private void Update()
    {
        CheckForInputs();
        CheckForGround();
    }


    void CheckForInputs()
    {
        GetMovementInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Wall jump takes priority when touching a wall
            if (!isGrounded && isTouchingWall)
            {
                WallJump();
            }
            else if (isGrounded)
            {
                Jump();

                // Reset the double jump when grounded
                jumpsRemaining = maxJumps - 1;
            }
            else if (jumpsRemaining > 0)
            {
                DoubleJump();
            }
        }
    }

    void GetMovementInput()
    {
        float directionInput = Input.GetAxisRaw("Horizontal");
        horizontalMovement = directionInput * moveSpeed;
    }

    void Jump()
    {
        // Reset vertical velocity so jumps are consistent
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector2.up * jumpHeight, ForceMode.Impulse);
    }

    void Gravity() //Because our proj is 3d we don't have access to the same rb.gravityScale that rb2D has, so we have to make a custum gravity specifically for our char. Other RB's will prob be fine w/ normal grav
    {
        float yVel = rb.velocity.y;
        
        if (Mathf.Abs(yVel) < lingeringAirTime) gravityScaling = lingerAtApexGravity;
        else if (yVel > 0) gravityScaling = risingGravity;
        else  gravityScaling =  fallingGravity;

        rb.AddForce(Vector3.up * Physics.gravity.y * gravityScaling, ForceMode.Acceleration);

        //Prob want to set a max fall speed here as well
    }

    void MovementPhysics()
    {
        Vector3 vel = Vector3.zero;
        vel.y = rb.velocity.y;
        if (isGrounded)
        {
            vel.x = horizontalMovement;
            rb.velocity = new Vector3(vel.x, vel.y, 0);
        }
        else
        {
            vel.x = Mathf.Lerp(rb.velocity.x, horizontalMovement, airControl);
            rb.velocity = new Vector3(vel.x, vel.y, 0);
        }
    }

    void DoubleJump()
    {
        // Reset vertical velocity so the double jump feels consistent
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(Vector2.up * jumpHeight, ForceMode.Impulse);

        jumpsRemaining--;
    }

    void WallJump()
    {
        // Push the player away from the wall
        rb.velocity = new Vector3(
            -wallDirection * wallJumpHorizontalForce,
            wallJumpForce,
            0
        );
    }

    void CheckForGround()
    {
        if (objectsUnderFeet.Count == 0)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
            gravityScaling = defaultGravity;
            // Restore jumps when touching the ground
            jumpsRemaining = maxJumps;
        }
    }

    void CheckForWall()
    {
        isTouchingWall = false;
        wallDirection = 0;

        // Check right
        RaycastHit rightHit;

        if (Physics.Raycast(
            transform.position,
            Vector3.right,
            out rightHit,
            wallCheckDistance,
            LayerMask.GetMask("Ground")))
        {
            isTouchingWall = true;
            wallDirection = 1;
            return;
        }

        // Check left
        RaycastHit leftHit;

        if (Physics.Raycast(
            transform.position,
            Vector3.left,
            out leftHit,
            wallCheckDistance,
            LayerMask.GetMask("Ground")))
        {
            isTouchingWall = true;
            wallDirection = -1;
        }
    }
}