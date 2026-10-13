using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounce : MonoBehaviour
{
    private EnemyMovement enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player =
            collision.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        if (player.IsInAir())
        {
            enemy.Stomp(player);
        }
    }
}
