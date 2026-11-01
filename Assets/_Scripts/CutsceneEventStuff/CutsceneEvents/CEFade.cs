using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CEFade : CutsceneEvent
{
    Color startCol;
    public Color endCol;
    public float fadeDuration;
    public SpriteRenderer sr;
    public AnimationCurve easing;


    public override void Enter()
    {
        startCol = sr.color;
        StartCoroutine(Fade());
    }
    public override void Do()
    {

    }
    public override void Exit()
    {

    }

    private IEnumerator Fade()
    {
        float elapsedTime = 0;


        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            t = easing.Evaluate(t);
            sr.color = Color.Lerp(startCol, endCol, t);
            yield return null;
        }

        sr.color = endCol;
    }
}
