using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public AudioClip finishedRepair;
    public GameObject repairEffect;
    public Sprite fixedSprite;
    public ParticleSystem crashedSmoke;
    public float flashTime;
    Material srMat;
    bool repaired;

    public override void Enter()
    {
        shipAS.clip = repairingSound;
        shipAS.Play();
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
            if(dustAmount < dustSequenceEnd - 1)
            {
                CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(dustShakeDur, dustShakeMag));
            }
        }
        else if (dustAmount >= dustSequenceEnd && !repaired)
        {
            StartCoroutine(RepairShip());
        }
    }

    public IEnumerator RepairShip()
    {
        shipAS.clip = finishedRepair;
        shipAS.Play();
        repaired = true;
        crashedSmoke.Stop();
        srMat = spriteRenderer.material;
        spriteRenderer.sprite = fixedSprite;
        CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(dustShakeDur*1.5f, dustShakeMag*3f));
        srMat.SetFloat("_Blend", 1);
        float elapsed = 0;
        Instantiate(repairEffect, dustParticleSpawnBounds.bounds.center, Quaternion.identity);

        while (elapsed < flashTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flashTime;
            float blend = Mathf.Lerp(1, 0, t);
            srMat.SetFloat("_Blend", blend);
            yield return null;
        }
        srMat.SetFloat("_Blend", 0);

    }
}
