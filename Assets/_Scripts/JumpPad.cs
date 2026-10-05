using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpPadForce;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();

        playerRB.velocity = new Vector3(playerRB.velocity.x, 0);
        playerRB.AddForce(Vector2.up * jumpPadForce, ForceMode2D.Impulse);
    }
}
