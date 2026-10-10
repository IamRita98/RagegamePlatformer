using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [Header("Left/Right Movement")]
    [SerializeField] private float moveSpeed;
    private float acceleration;
    [SerializeField]private float defaultAcceleration;
    [SerializeField]private float iceAcceleration = .01f;

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float airControl = .2f;
    [SerializeField] private float risingGravity = 4;
    [SerializeField] private float fallingGravity = 7;
    [SerializeField] private float lingerAtApexGravity = .7f;
    [SerializeField] private float lingeringAirTime; //This is not 1 to 1 of value to seconds, so play around w/ it
    private float defaultGravity = 1;
    [SerializeField] float gravityScaling;
    public bool launched = false;
    float launchTimer;
    [SerializeField]float jumpPadLockoutTimer;
    

    // Double jump
    [SerializeField] private int maxJumps = 1;
    private int jumpsRemaining;
    [HideInInspector] public bool jumpPressed;

    // Wall jump
    [SerializeField] private float wallJumpForce = 8f;
    [SerializeField] private float wallJumpHorizontalForce = 8f;
    [SerializeField] private float wallCheckDistance = 0.6f;

    public float deathVelocity;

    [Header("Gun")]
    [SerializeField] float fireRate;
    [SerializeField] private GameObject gunPos;
    [SerializeField] GameObject bullet;
    [HideInInspector] public bool gunOnCD;
    float gunTimer;

    [Header("Enemy")]
    [SerializeField] float bounceVel = 3f;

    public Rigidbody2D rb;
    [HideInInspector] public float horizontalMovement;


    public bool isTouchingWall;
    [HideInInspector] public int wallDirection;

    [Header("Sprite renderers")]
    public List<SpriteRenderer> spriteRenderersList;

    [Header("Ground Check")]
    public bool isGrounded;

    public Transform groundCollPos;
    [SerializeField] private Vector3 groundCheckBoxSize = new Vector3(.3f, .06f, .01f);
    public List<Collider2D> objectsUnderFeet = new List<Collider2D>();

    float directionInput;
    [HideInInspector] public bool aimingUp;
    [HideInInspector] public bool isFacingRight;

    [Header("State Machine")]
    public StateMachine sm;
    public bool IsDead => sm.currentState == sm.deathState;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        jumpsRemaining = maxJumps;
        acceleration = defaultAcceleration;
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

        if(!IsDead) CheckForWall();
    }

    private void Update()
    {
        if(!IsDead) CheckForInputs();
        CheckForGround();
        if (gunOnCD) gunTimer += Time.deltaTime;
        if (gunTimer >= fireRate) gunOnCD = false;

        if (launched) launchTimer -= Time.deltaTime;
        if (launchTimer <= 0) launched = false;
    }


    void CheckForInputs()
    {
        #region AIM UP
        Vector2 gunLocPos = gunPos.transform.localPosition;
        if (Input.GetAxis("Vertical") > 0 && sm.currentState != sm.wallClingState)
        {
            aimingUp = true;
            gunLocPos.y = 0.25f;
            if (isFacingRight)
            {
                gunLocPos.x = -0.2f;
            }
            else
            {
                gunLocPos.x = 0.2f;
            }
        }
        else
        {
            aimingUp = false;
            gunLocPos.y = 0f;
            gunLocPos.x = 0f;
        }
        gunPos.transform.localPosition = gunLocPos;

        #endregion

        #region LEFT/RIGHT
        directionInput = Input.GetAxisRaw("Horizontal");
        horizontalMovement = directionInput * moveSpeed;
        if (directionInput > 0)
        {
            if(sm.currentState == sm.wallClingState)
            {
                isFacingRight = false;
            }
            else
            {
                isFacingRight = true;
                FlipSrX();
            }
        }
        else if(directionInput < 0)
        {
            if (sm.currentState == sm.wallClingState)
            {
                isFacingRight = true;
            }
            else
            {
                isFacingRight = false;
                FlipSrX();
            }

        }
        #endregion

        #region JUMP
        if (Input.GetAxis("Jump") > 0 && !jumpPressed)
        {
            jumpPressed = true;
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

        if(Input.GetAxis("Jump") == 0)
        {
            jumpPressed = false;
        }
        #endregion

        #region GUN
        if (sm.currentState != sm.landState && Input.GetAxis("Shoot") > 0 && !gunOnCD)
        {
            GameObject bulletGO = Instantiate(bullet, gunPos.transform.position, Quaternion.identity);
            BulletBehaviour bBehaviour = bulletGO.GetComponent<BulletBehaviour>();

            Vector2 direction = Vector2.zero;

            if (aimingUp && sm.currentState != sm.wallClingState)
            {
                direction.y = 1;
            }
            else if (isFacingRight)
            {
                direction.x = 1;
            }
            else if (!isFacingRight)
            {
                direction.x = -1;
            }
            bBehaviour.direction = direction;

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
        if(sm.currentState != sm.wallClingState)
        {
            float yVel = rb.velocity.y;
            if (Mathf.Abs(yVel) < lingeringAirTime) gravityScaling = lingerAtApexGravity;
            else if (yVel > 0) gravityScaling = risingGravity;
            else gravityScaling = fallingGravity;

            rb.gravityScale = gravityScaling;
        }
        //rb.AddForce(Vector3.up * Physics.gravity.y * gravityScaling);

        //Prob want to set a max fall speed here as well
    }

    void MovementPhysics()
    {
        Vector3 vel;
        vel.y = rb.velocity.y;
        if (isGrounded)
        {
            if (launched) return;
            vel.x = Mathf.Lerp(rb.velocity.x, horizontalMovement, acceleration);
            rb.velocity = new Vector3(vel.x, vel.y, 0);
        }
        else
        {
            if (launched) return;
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
            if (objectsUnderFeet[0].gameObject.CompareTag("Ice")) acceleration = iceAcceleration;
            else acceleration = defaultAcceleration;
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

        Vector2 direction = Vector2.zero;
        direction.x = isFacingRight ? 1 : -1;
        
        if (Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, LayerMask.GetMask("Vines")))
        {
            isTouchingWall = true;
            wallDirection = 1;
            return;
        }

        //Check left
        if (Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, LayerMask.GetMask("Vines")))
        {
            isTouchingWall = true;
            wallDirection = -1;
            return;
        }
    }

    public void FlipSrX()
    {
        foreach (SpriteRenderer sr in spriteRenderersList)
        {
            if (isFacingRight)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
    }

    public void Launched()
    {
        launched = true;
        launchTimer = jumpPadLockoutTimer;
    }

    // Checks if player is jumping on an enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("KillPlayer"))
        {
            sm.isDead = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyMovement enemy =
            collision.gameObject.GetComponentInParent<EnemyMovement>();

        if (enemy == null)
            return;

        bool playerIsAboveEnemy =
            transform.position.y > enemy.transform.position.y;

        if (playerIsAboveEnemy && sm.currentState == sm.inAirState && enemy.isStompable)
        {
            enemy.gameObject.SetActive(false);

            rb.velocity = new Vector2(
                rb.velocity.x,
                bounceVel
            );
        }
        else
        {
            sm.isDead = true;
        }
    }

}