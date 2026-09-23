using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;

    // Double jump
    [SerializeField] private int maxJumps = 2;
    private int jumpsRemaining;

    // Wall jump
    [SerializeField] private float wallJumpForce = 8f;
    [SerializeField] private float wallJumpHorizontalForce = 8f;
    [SerializeField] private float wallCheckDistance = 0.6f;

    [SerializeField] private Vector3 groundCheckBoxSize = new Vector3(.3f, .06f, .01f);

    Rigidbody rb;
    float horizontalMovement;

    public Transform groundCollPos;

    bool isGrounded;
    bool isTouchingWall;
    int wallDirection;

    List<Collider> objectsUnderFeet = new List<Collider>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpsRemaining = maxJumps;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalMovement, rb.velocity.y, 0);

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
        Movement();

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

    void Movement()
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