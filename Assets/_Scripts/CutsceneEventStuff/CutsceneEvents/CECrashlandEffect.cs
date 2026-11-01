using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CECrashlandEffect : CutsceneEvent
{
    [Header("Event Specific attributes")]
    public float shakeMag;
    public float shakeDur;
    public GameObject effectsPos;
    public GameObject crashSmoke;

    public SpriteRenderer shipSr;
    public Sprite brokenShipSprite;

    public int debrisAmount;
    public float debrisVelMin;
    public float debrisVelMax;
    public float debrisAngleMin;
    public float debrisAngleMax;

    public ParticleSystem smokeEffectToDeactivate;
    public ParticleSystem smokeEffectToActivate;

    public List<GameObject> debris;

    public override void Enter()
    {
        BoxCollider2D ePosBC = effectsPos.GetComponent<BoxCollider2D>();
        Bounds spawnBounds = ePosBC.bounds;

        CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(shakeDur, shakeMag));

        Instantiate<GameObject>(crashSmoke, effectsPos.transform.position, Quaternion.identity);

        for (int i = 0; i < debrisAmount; i++)
        {

            float randomX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
            float randomY = Random.Range(spawnBounds.min.y, spawnBounds.max.y);

            Vector3 spawnPos = new Vector3(randomX, randomY, 0);

            GameObject newDebris = Instantiate<GameObject>(debris[Random.Range(0, debris.Count - 1)], spawnPos, Quaternion.identity);

            Rigidbody2D rb = newDebris.GetComponent<Rigidbody2D>();

            rb.angularVelocity = Random.Range(debrisVelMin, debrisVelMax);

            Vector2 newVel = Random.insideUnitCircle.normalized * Random.Range(debrisVelMin, debrisVelMax);
            rb.velocity = newVel;
        }

        smokeEffectToDeactivate.Stop();
        smokeEffectToActivate.Play();
        shipSr.sprite = brokenShipSprite;
    }
    public override void Do()
    {

    }
    public override void Exit()
    {

    }


}
