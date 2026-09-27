using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float lifeTime;
    float timer;
    public float dir;

    private void Awake()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifeTime) Destroy(gameObject);
        transform.Translate(new Vector3(dir, 0, 0) * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.GetComponent<EnemyMovement>().isShootable)
            Destroy(other.gameObject);
    }
}
