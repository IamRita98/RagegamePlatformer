using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterPlayerCollision : MonoBehaviour
{
    SpriteRenderer sRend;
    Collider2D col;
    Rigidbody2D rb;
    [SerializeField] float timeToDestroy;
    float timer;
    bool startTimer = false;
    Vector2 startPos;

    private void Awake()
    {
        sRend = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        timer = timeToDestroy;
        startPos = transform.position;
    }

    private void Start()
    {
        StateMachine.Instance.onPlayerRespawn.AddListener(TurnOn);
    }

    private void OnDestroy()
    {
        if (StateMachine.Instance != null)
        {
            StateMachine.Instance.onPlayerRespawn.RemoveListener(TurnOn);
        }
    }

    private void Update()
    {
        if (startTimer)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                TurnOff();
            }
        }
    }

    void TurnOff()
    {
        sRend.enabled = false;
        col.enabled = false;
        rb.velocity = Vector2.zero;
        startTimer = false;
    }

    void TurnOn()
    {
        sRend.enabled = true;
        col.enabled = true;
        transform.position = startPos;
        timer = timeToDestroy;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        startTimer = true;
    }
}
