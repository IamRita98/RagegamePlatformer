using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalVelocityCollider : MonoBehaviour
{
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(rb.velocity.magnitude >= PlayerController.Instance.deathVelocity) StateMachine.Instance.isDead = true;
        }
    }
}
