using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReflect : MonoBehaviour
{
    public bool reflectIsActive = false;
    EnemyMovement enemyMovement;
    [SerializeField] float addedReflectedBulletSpeed;

    private void Start()
    {
        if (reflectIsActive == true) enemyMovement.isShootable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!reflectIsActive) return;
        if (!collision.gameObject.CompareTag("Bullet")) return;
        BulletBehaviour bullet = collision.gameObject.GetComponent<BulletBehaviour>();
        Rigidbody2D bulletRB = collision.gameObject.GetComponent<Rigidbody2D>();
        Vector2 bulletDir = new Vector2(bullet.direction.x * -1, bullet.direction.y);
        bulletRB.velocity = bulletDir * (bullet.bulletSpeed + addedReflectedBulletSpeed);
        bullet.canHitPlayer = true;
    }
}
