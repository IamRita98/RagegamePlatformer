using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    public Vector2 originalLocalPos = Vector2.zero;
    public GameObject cameraGO;
    public AnimationCurve easing;
    bool shaking;

    private void Awake()
    {
        Instance = this;
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

            cameraGO.transform.localPosition = originalLocalPos + randomOffset;

            elapsed += Time.deltaTime;

            yield return null;
        }

        shaking = false;
        cameraGO.transform.localPosition = originalLocalPos;
    }
}
