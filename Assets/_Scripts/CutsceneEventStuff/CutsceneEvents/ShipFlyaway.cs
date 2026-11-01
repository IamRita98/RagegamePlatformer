using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ShipFlyaway : CutsceneEvent
{
    public List<GameObject> thrusters;
    public AudioSource audioSource;
    public float shakeDur;
    public float shakeMag;
    public AnimationCurve easing;
    public GameObject shakeGO;

    Vector2 originalLocalPos = Vector2.zero;
    bool shaking;
    public override void Enter()
    {
        for(int i = 0; i < thrusters.Count; i++)
        {
            thrusters[i].SetActive(true);
            StartCoroutine(Shake(shakeDur, shakeMag));
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (shaking) yield break;
        float elapsed = 0;
        shaking = true;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            Vector2 randomOffset = Random.insideUnitCircle * (magnitude * easing.Evaluate(t));

            shakeGO.transform.localPosition = originalLocalPos + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        shaking = false;
        shakeGO.transform.localPosition = originalLocalPos;
    }
}
