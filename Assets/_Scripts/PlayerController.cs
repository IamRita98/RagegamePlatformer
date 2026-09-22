using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;

    Rigidbody rb;
    float horizontalMovement;
    float verticalMovement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalMovement, rb.velocity.y, 0);
    }

    private void Update()
    {
        CheckForInputs();
    }

    void CheckForInputs()
    {
        Movement();
        //Shoot
        //If !onGround  DoubleJump/WallJump?
        //else Jump
    }

    void Movement()
    {
        float directionInput = Input.GetAxisRaw("Horizontal");
        horizontalMovement = directionInput * moveSpeed;
    }
}
