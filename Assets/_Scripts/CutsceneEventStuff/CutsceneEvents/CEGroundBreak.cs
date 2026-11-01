using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEGroundBreak : CutsceneEvent
{
    public float shakeDur;
    public float shakeMag;

    public GameObject groundToDestroy;
    public GameObject groundDustEffect;
    
    public override void Enter()
    {
        Instantiate(groundDustEffect, groundToDestroy.transform.position, Quaternion.identity);
        Destroy(groundToDestroy);
        CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(shakeDur, shakeMag));
    }
    public override void Do()
    {

    }
    public override void Exit()
    {

    }
}
