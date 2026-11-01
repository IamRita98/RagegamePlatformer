using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CECreateDebris : CutsceneEvent
{
    public GameObject debrisGO;
    public GameObject spawnPos;
    public Vector2 vel;

    public override void Enter()
    {
        GameObject spawnedGO = Instantiate(debrisGO, spawnPos.transform.position, Quaternion.identity);
        Rigidbody2D rb = spawnedGO.GetComponent<Rigidbody2D>();
        rb.AddForce(vel, ForceMode2D.Impulse);
        rb.angularVelocity = Random.Range(20, 40);
    }
}
