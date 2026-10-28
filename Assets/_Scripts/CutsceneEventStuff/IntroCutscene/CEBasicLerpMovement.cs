using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEBasicLerpMovement : CutsceneEvent
{
    [Header("Event Specific attributes")]
    public Vector3 initialPos;
    public Vector3 endPos;
    public GameObject moveGO;
    public AnimationCurve easing;

    float elapsedTime;

    public override void Enter()
    {

    }
    public override void Do()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration;
        t = easing.Evaluate(t);
        Vector3 pos = moveGO.transform.position;
        pos = Vector3.Lerp(initialPos, endPos, t);
        moveGO.transform.position = pos;
    }
    public override void Exit()
    {

    }
}
