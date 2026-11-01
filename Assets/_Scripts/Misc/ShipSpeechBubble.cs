using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipSpeechBubble : MonoBehaviour
{
    public GameObject speechBubble;
    public AnimationCurve easing;
    public float transitionSpeed;
    public float t;
    public float targetT;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetT = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetT = 0;
        }
    }

    private void Update()
    {
        if(t != targetT)
        {
            t = Mathf.MoveTowards(t, targetT, transitionSpeed * Time.deltaTime);
            float u = easing.Evaluate(t);
            Vector3 newScale = new Vector3(u, u, u);
            speechBubble.transform.localScale = newScale;
        }

    }
}
