using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

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

    [Header("Gun")]
    [SerializeField] float fireRate;
    [SerializeField] private GameObject gunPos;
    [SerializeField] GameObject bullet;
    bool gunOnCD;
    float gunTimer;

    [Header("Enemy")]
    [SerializeField] float bounceVel = 3f;

    public Rigidbody2D rb;
    float horizontalMovement;


    bool isTouchingWall;
    int wallDirection;

    [Header("Ground Check")]
    public bool isGrounded;

    public Transform groundCollPos;
    [SerializeField] private Vector3 groundCheckBoxSize = new Vector3(.3f, .06f, .01f);
    public List<Collider2D> objectsUnderFeet = new List<Collider2D>();

    float directionInput;
    bool isFacingRight;

    private bool stomped = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = maxJumps;
    }

    private void FixedUpdate()
    {
        Gravity();
        MovementPhysics();

        objectsUnderFeet = Physics2D.OverlapBoxAll(
            groundCollPos.position,
            groundCheckBoxSize,
            0f,
            LayerMask.GetMask("Ground")
        ).ToList();

        CheckForWall();
    }

    private void Update()
    {
        CheckForInputs();
        CheckForGround();
        if (gunOnCD) gunTimer += Time.deltaTime;
        if (gunTimer >= fireRate) gunOnCD = false;
    }


    void CheckForInputs()
    {
        #region LEFT/RIGHT
        directionInput = Input.GetAxisRaw("Horizontal");
        horizontalMovement = directionInput * moveSpeed;
        if (directionInput == 1)
        {
            isFacingRight = true;
        }
        else if(directionInput == -1)
        {
            isFacingRight = false;
        }
        #endregion

        #region JUMP
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
        #endregion

        #region GUN
        if (Input.GetKeyDown(KeyCode.Z) && !gunOnCD)
        {
            GameObject bulletGO = Instantiate(bullet, gunPos.transform.position, Quaternion.identity);
            BulletBehaviour bBehaviour = bulletGO.GetComponent<BulletBehaviour>();
            if (isFacingRight) bBehaviour.dir = 1;
            else bBehaviour.dir = -1;
            gunOnCD = true;
            gunTimer = 0;
        }
        #endregion
    }

    void Jump()
    {
        // Reset vertical velocity so jumps are consistent
        rb.velocity = new Vector3(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }

    void Gravity()
    {
        float yVel = rb.velocity.y;
        
        if (Mathf.Abs(yVel) < lingeringAirTime) gravityScaling = lingerAtApexGravity;
        else if (yVel > 0) gravityScaling = risingGravity;
        else  gravityScaling =  fallingGravity;

        rb.gravityScale = gravityScaling;
        //rb.AddForce(Vector3.up * Physics.gravity.y * gravityScaling);

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
        rb.velocity = new Vector3(rb.velocity.x, 0);

        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);

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

    // Checks if player is jumping on an enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.GetComponent<EnemyMovement>().isStompable)
        {
            Destroy(collision.gameObject);
            rb.velocity = new Vector3(rb.velocity.x, bounceVel);
            stomped = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !stomped)
        {
            Destroy(this.gameObject);
        }
        else if (stomped)
            stomped = false;
    }
}