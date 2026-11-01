using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingBlockBehaviour : MonoBehaviour
{
    public bool resetOnPDeath = true;
    Rigidbody2D rb;
    [SerializeField] float bulletForce;
    GameObject player;
    Vector2 startingPos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
    }

    private void OnEnable()
    {
        StateMachine.Instance.onPlayerRespawn.AddListener(ResetPos);
    }

    private void OnDisable()
    {
        StateMachine.Instance.onPlayerRespawn.RemoveListener(ResetPos);
    }

    void ResetPos()
    {
        transform.position = startingPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Bullet")) return;
        Vector2 bulletDir = Vector2.left;
        if (player.transform.position.x < transform.position.x) bulletDir = Vector2.right;

        rb.AddForce(bulletForce * bulletDir);
    }
}
