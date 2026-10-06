using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private int startingDirection = 1;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Whether or not this enemy is killed when jumped on, shot at, both, or neither
    public bool isStompable;
    public bool isShootable;
    public bool walksOff;

    private float wallCheckDistance = 1f;
    private bool isTouchingWall;
    private bool isGroundAhead;
    private int currentDirection;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        currentDirection = startingDirection;

        groundCheck.localPosition = new Vector2(
            Mathf.Abs(groundCheck.localPosition.x) * currentDirection,
            groundCheck.localPosition.y
        );

        spriteRenderer.flipX = currentDirection == 1;
    }

    private void FixedUpdate()
    {
        CheckForWall();
        CheckForFloor();

        bool lavaAhead = CheckForLava();

        if (isTouchingWall || lavaAhead || (!walksOff && !isGroundAhead))
        {
            ChangeDirection();
        }

        rb.velocity = new Vector2(
            currentDirection * movementSpeed,
            rb.velocity.y
        );
    }

    // I stole this from PlayerController script
    void CheckForWall()
    {
        Vector2 direction = currentDirection == 1
            ? Vector2.right
            : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            wallCheckDistance,
            LayerMask.GetMask("Ground")
        );

        isTouchingWall = hit.collider != null;

        Debug.DrawRay(
            transform.position,
            direction * wallCheckDistance,
            Color.red
        );
    }

    void CheckForFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            wallCheckDistance,
            groundLayer
        );

        isGroundAhead = hit.collider != null;

        Debug.DrawRay(
            groundCheck.position,
            Vector2.down * wallCheckDistance,
            Color.green
        );
    }

    bool CheckForLava()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            groundCheck.position,
            Vector2.down,
            wallCheckDistance
        );

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Lava"))
                return true;
        }

        return false;
    }

    void ChangeDirection()
    {
        currentDirection *= -1;

        spriteRenderer.flipX = currentDirection == 1;

        groundCheck.localPosition = new Vector2(
            Mathf.Abs(groundCheck.localPosition.x) * currentDirection,
            groundCheck.localPosition.y
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ChangeDirection();
        }
    }
}
