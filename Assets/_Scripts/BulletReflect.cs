using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReflect : MonoBehaviour
{
    public bool reflectIsActive = false;
    EnemyMovement enemyMovement;

    private void Start()
    {
        if (reflectIsActive == true) enemyMovement.isShootable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!reflectIsActive) return;
        if (!collision.gameObject.CompareTag("Bullet")) return;
        BulletBehaviour bullet = collision.gameObject.GetComponent<BulletBehaviour>();
        bullet.direction = new Vector2(bullet.direction.x * -1, bullet.direction.y);
        bullet.canHitPlayer = true;
    }
}
