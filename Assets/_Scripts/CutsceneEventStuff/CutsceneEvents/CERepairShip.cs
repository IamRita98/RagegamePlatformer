using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CERepairShip : CutsceneEvent
{
    [Header("Initialization")]
    public AudioSource shipAS;
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D dustParticleSpawnBounds;

    [Header("Repairing")]
    public AudioClip repairingSound;
    public GameObject dustEffect;
    public float dustFrequency;
    public float dustShakeMag;
    public float dustShakeDur;
    public int dustSequenceEnd;
    int dustAmount;
    float elapsed;

    [Header("Repair")]
    public GameObject repairEffect;
    public Sprite fixedSprite;
    public ParticleSystem crashedSmoke;

    public override void Enter()
    {
        //    shipAS.clip = repairingSound;
        //    shipAS.Play();
    }
    public override void Do()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= dustFrequency && dustAmount < dustSequenceEnd)
        {
            dustAmount++;
            elapsed = 0;
            Vector2 centerPos = dustParticleSpawnBounds.bounds.center;
            float maxScale = Mathf.Max(dustParticleSpawnBounds.transform.lossyScale.x, dustParticleSpawnBounds.transform.lossyScale.y);
            float worldRadius = dustParticleSpawnBounds.radius * maxScale;

            Vector2 randomPoint = Random.insideUnitCircle * worldRadius;
            Vector2 spawnPosition = centerPos + randomPoint;
            Instantiate(dustEffect, spawnPosition, Quaternion.identity);
            CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(dustShakeDur, dustShakeMag));
        }
        else if (dustAmount >= dustSequenceEnd)
        {
            
        }
    }

    public IEnumerator repairShip()
    {
    
    }
}
