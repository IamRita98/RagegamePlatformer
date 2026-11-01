using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEffectBasic : MonoBehaviour
{
    public float destroyTime;
    public float upVel;
    public float velXMin;
    public float velXMax;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 velocity = new Vector2(Random.Range(velXMin, velXMax),upVel);
        rb.AddForce(velocity, ForceMode2D.Impulse);
    }

    private void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0) Destroy(gameObject);
    }
}
