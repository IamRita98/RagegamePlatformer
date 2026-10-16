using UnityEngine;

public class SpikeCheck : MonoBehaviour
{
    private EnemyMovement enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.ActivateSpike();
        }
    }
}