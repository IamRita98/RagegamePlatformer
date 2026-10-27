using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingBlockBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float bulletForce;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

/*    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Bullet")) return;
        Vector2 bulletDir = Vector2.left;
        if (player.transform.position.x < transform.position.x) bulletDir = Vector2.right;

        rb.AddForce(bulletForce * bulletDir);
    }
}
