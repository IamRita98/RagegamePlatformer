using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    /// <summary>
    /// Currently jumpHeight is more just a force added to the players upward velocity. We need to make something more complicated for the number assigned to this variable
    /// to directly equate to the height of the jump
    /// </summary>
    [SerializeField] private float jumpHeight;
    /// <summary>
    /// This is how far the box extends in each direction from the center, so this value * 2 would be its total size
    /// </summary>
    [SerializeField] private Vector3 groundCheckBoxSize = new Vector3(.3f, .06f, .01f);

    Rigidbody rb;
    float horizontalMovement;
    public Transform groundCollPos;
    bool isGrounded;
    List<Collider> objectsUnderFeet = new List<Collider>();


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalMovement, rb.velocity.y, 0);
        objectsUnderFeet = Physics.OverlapBox(groundCollPos.position, groundCheckBoxSize, Quaternion.identity, LayerMask.GetMask("Ground")).ToList();
    }

    private void Update()
    {
        CheckForInputs();
        CheckForGround();
    }

    void CheckForInputs()
    {
        Movement();
        // if(Press Shoot)Shoot()
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isGrounded) return; //Eventually, double jump/walljump
            else Jump();
        }
    }

    void Movement()
    {
        float directionInput = Input.GetAxisRaw("Horizontal");
        horizontalMovement = directionInput * moveSpeed;
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpHeight);
    }

    void CheckForGround()
    {
        if (objectsUnderFeet.Count == 0) isGrounded = false;
        else isGrounded = true;
    }
}
