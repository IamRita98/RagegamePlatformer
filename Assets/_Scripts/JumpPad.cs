using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpPadForce;
    [SerializeField] enum DirectionToLaunch
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
    [SerializeField] DirectionToLaunch directionToLaunch;
    [SerializeField] bool bulletCanChangeDirection;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
        
        playerRB.velocity = new Vector3(playerRB.velocity.x, 0);
        switch (directionToLaunch)
        {
            case DirectionToLaunch.Up: playerRB.AddForce(Vector2.up * jumpPadForce, ForceMode2D.Impulse); break;
            case DirectionToLaunch.Down: playerRB.AddForce(Vector2.down * jumpPadForce, ForceMode2D.Impulse); break;
            case DirectionToLaunch.Left: playerRB.AddForce(Vector2.left * jumpPadForce, ForceMode2D.Impulse); break;
            case DirectionToLaunch.Right: playerRB.AddForce(Vector2.right * jumpPadForce, ForceMode2D.Impulse); break;
            case DirectionToLaunch.UpLeft: playerRB.AddForce(new Vector2(-1, 1).normalized * jumpPadForce, ForceMode2D.Impulse); break;
            case DirectionToLaunch.UpRight: playerRB.AddForce(new Vector2(1, 1).normalized * jumpPadForce, ForceMode2D.Impulse); break;
            case DirectionToLaunch.DownLeft: playerRB.AddForce(new Vector2(-1, -1).normalized * jumpPadForce, ForceMode2D.Impulse); break;
            case DirectionToLaunch.DownRight: playerRB.AddForce(new Vector2(1, -1).normalized * jumpPadForce, ForceMode2D.Impulse); break;
        }
        PlayerController.Instance.Launched();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!bulletCanChangeDirection) return;
        if (!collision.gameObject.CompareTag("Bullet")) return;
        ChangeDirection();
    }

    void ChangeDirection()
    {
        int count = System.Enum.GetValues(typeof(DirectionToLaunch)).Length;
        directionToLaunch = (DirectionToLaunch)(((int)directionToLaunch + 1) % count); //ngl, I had no idea how to iterate through an enum, so if this part of the code looks sus its because its straight from Claude...
    }
}
