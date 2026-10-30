using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpPad : MonoBehaviour
{
    public float jumpPadForce;


    [SerializeField]
    private enum DirectionToLaunch
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


    [SerializeField] private DirectionToLaunch directionToLaunch;
    [SerializeField] private bool bulletCanChangeDirection;


    [Header("Jump Pad Audio")]
    [SerializeField] private AudioSource jumpPadAudio;


    private void Awake()
    {
        if (jumpPadAudio == null)
        {
            jumpPadAudio = GetComponent<AudioSource>();
        }


        if (jumpPadAudio != null)
        {
            jumpPadAudio.playOnAwake = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;


        Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();


        if (playerRB == null)
            return;


        // Play sound every time the player hits the jump pad
        if (jumpPadAudio != null)
        {
            jumpPadAudio.PlayOneShot(jumpPadAudio.clip);
        }


        playerRB.velocity = new Vector2(playerRB.velocity.x, 0);


        switch (directionToLaunch)
        {
            case DirectionToLaunch.Up:
                playerRB.AddForce(Vector2.up * jumpPadForce, ForceMode2D.Impulse);
                break;


            case DirectionToLaunch.Down:
                playerRB.AddForce(Vector2.down * jumpPadForce, ForceMode2D.Impulse);
                break;


            case DirectionToLaunch.Left:
                playerRB.AddForce(Vector2.left * jumpPadForce, ForceMode2D.Impulse);
                break;


            case DirectionToLaunch.Right:
                playerRB.AddForce(Vector2.right * jumpPadForce, ForceMode2D.Impulse);
                break;


            case DirectionToLaunch.UpLeft:
                playerRB.AddForce(new Vector2(-1, 1).normalized * jumpPadForce, ForceMode2D.Impulse);
                break;


            case DirectionToLaunch.UpRight:
                playerRB.AddForce(new Vector2(1, 1).normalized * jumpPadForce, ForceMode2D.Impulse);
                break;


            case DirectionToLaunch.DownLeft:
                playerRB.AddForce(new Vector2(-1, -1).normalized * jumpPadForce, ForceMode2D.Impulse);
                break;


            case DirectionToLaunch.DownRight:
                playerRB.AddForce(new Vector2(1, -1).normalized * jumpPadForce, ForceMode2D.Impulse);
                break;
        }


        PlayerController.Instance.Launched();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!bulletCanChangeDirection)
            return;


        if (!collision.gameObject.CompareTag("Bullet"))
            return;


        ChangeDirection();
    }


    private void ChangeDirection()
    {
        int count = System.Enum.GetValues(typeof(DirectionToLaunch)).Length;


        directionToLaunch = (DirectionToLaunch)(
            ((int)directionToLaunch + 1) % count
        );
    }
}
