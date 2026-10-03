using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxTrap : MonoBehaviour
{
    [Tooltip("The box trigger")]
    public BoxCollider2D bc;

    [Tooltip("The Game Object that will be moved \nIts important that this object is a child of the ProxTrap object")]
    public GameObject trapGO;

    [Tooltip("Where the object will be moved to relative to its starting position")]
    public Vector3 endPosition;
    private Vector3 startPosition;

    public float moveTime = 0.5f;

    [Tooltip("Adjust this curve if you want the movement to have easing")]
    public AnimationCurve movementCurve;

    [Tooltip("If you want the trap to reset, enter a value here otherwise leave it at zero")]
    public float resetTime = 0;

    public float resetMoveTime = 1;

    private bool activated = false;

    private void Awake()
    {
        startPosition = trapGO.transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !activated)
        {
            StartCoroutine(MoveTrap(moveTime, endPosition));
        }
    }

    private IEnumerator MoveTrap(float tDuration, Vector3 endPos)
    {
        activated = (activated)? false: true;

        Vector3 startPos = trapGO.transform.localPosition;

        float tElapsed = 0;
        while(tElapsed < tDuration)
        {
            tElapsed += Time.deltaTime;
            float t = tElapsed / tDuration;
            float tCurve = movementCurve.Evaluate(t);

            trapGO.transform.localPosition = Vector3.Lerp(startPos, endPos, tCurve);

            yield return null;
        }

        trapGO.transform.localPosition = endPos;

        if(resetTime != 0 && activated)
        {
            yield return new WaitForSeconds(resetTime);
            StartCoroutine(MoveTrap(resetMoveTime, startPosition));
        }

    }
}
