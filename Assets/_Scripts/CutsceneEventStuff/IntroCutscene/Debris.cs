using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{

    public float duration;
    float elapsed;
    float elapsedScale;
    Vector3 startScale = new Vector3(1, 1, 1);


    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration * 0.5f)
        {
            elapsedScale += Time.deltaTime;
            float t = elapsedScale/(duration * 0.5f);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
        }

        if(elapsed >= duration)
        {
            Destroy(gameObject);
        }
    }
}
