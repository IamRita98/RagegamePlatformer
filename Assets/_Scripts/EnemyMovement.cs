using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private int startingDirection = 1;
    [SerializeField] private float wallCheckDistance = 0.6f;

    // Whether or not this enemy is killed when jumped on, shot at, both, or neither
    public bool isStompable;
    public bool isShootable;

    private bool isTouchingWall;
    private int currentDirection;
    Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDirection = startingDirection;
    }

    private void FixedUpdate()
    {
        CheckForWall();
        if (isTouchingWall)
        {
            currentDirection *= -1;
        }

        rb.velocity = new Vector3(currentDirection * movementSpeed, rb.velocity.y, 0);
    }

    // I stole this from PlayerController script
    void CheckForWall()
    {
        isTouchingWall = false;

        // Check right
        //RaycastHit rightHit;

        if (Physics2D.Raycast(
            transform.position,
            Vector3.right,
            wallCheckDistance,
            LayerMask.GetMask("Ground")))
        {
            print("Test");
            isTouchingWall = true;
            return;
        }

        // Check left
        //RaycastHit leftHit;

        if (Physics2D.Raycast(
            transform.position,
            Vector3.left,
            wallCheckDistance,
            LayerMask.GetMask("Ground")))
        {
            isTouchingWall = true;
        }
    }
}
