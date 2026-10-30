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

    public float movementDuration;

    public override void Enter()
    {
        StartCoroutine(Move());
    }
    public override void Do()
    {

    }
    public override void Exit()
    {

    }

    private IEnumerator Move()
    {
        float elapsedTime = 0;


        while (elapsedTime < movementDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / movementDuration;
            t = easing.Evaluate(t);
            Vector3 pos = moveGO.transform.position;
            pos = Vector3.Lerp(initialPos, endPos, t);
            moveGO.transform.position = pos;
            yield return null;
        }
        moveGO.transform.position = endPos;
    }
}
