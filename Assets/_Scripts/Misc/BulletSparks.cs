using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSparks : MonoBehaviour
{
    public Rigidbody2D rb;

    public float stopTimeMin;
    public float stopTimeMax;
    float stopTime;

    public float velMin;
    public float velMax;
    Vector2 vel;


    private void Awake()
    {
        stopTime = Random.Range(stopTimeMin, stopTimeMax);
        vel = Random.insideUnitCircle.normalized;
        vel *= Random.Range(velMin, velMax);

        rb.AddForce(vel, ForceMode2D.Impulse);
    }

    private void Update()
    {
        stopTime -= Time.deltaTime;
        if (stopTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
