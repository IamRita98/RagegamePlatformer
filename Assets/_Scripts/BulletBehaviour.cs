using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float lifeTime;
    float timer;
    //public float dir;
    public Vector2 direction;
    private void Awake()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifeTime) Destroy(gameObject);
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) Destroy(gameObject);
        if (collision.CompareTag("Enemy"))
        {
            EnemyMovement em = collision.gameObject.GetComponent<EnemyMovement>();
            if (em.isShootable)
            {
                em.Die();
                Destroy(gameObject);
            }
        }
    }
}
