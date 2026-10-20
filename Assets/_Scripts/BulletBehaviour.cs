using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float lifeTime;
    public Rigidbody2D rb;
    public TrailRenderer tr;
    public GameObject sparksEffectGO;
    public int maxSparks;
    public int minSparks;
    float timer;
    //public float dir;
    public Vector2 direction;

    Vector2 lastPos;
    private void Awake()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifeTime) BulletHit();

    }
    private void FixedUpdate()
    {
        lastPos = transform.position;
        transform.Translate(direction * bulletSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) BulletHit();
        if (collision.CompareTag("Enemy"))
        {
            EnemyMovement em = collision.gameObject.GetComponent<EnemyMovement>();
            if (em.isShootable)
            {
                em.Die();
                BulletHit();
            }
        }
    }



    void BulletHit()
    {
        int sparksAmount = Random.Range(minSparks, maxSparks);

        for (int i = 0; i < sparksAmount; i++)
        {
            Instantiate(sparksEffectGO, lastPos, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
