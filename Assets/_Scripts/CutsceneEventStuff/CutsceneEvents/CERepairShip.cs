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

    [Header("Repair")]
    public GameObject repairEffect;
    public Sprite fixedSprite;
    public ParticleSystem crashedSmoke;

    public override void Enter()
    {
        shipAS.clip = repairingSound;
        shipAS.Play();
    }
    public override void Do()
    {

    }


}
