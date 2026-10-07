using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    static CameraMover Instance;
    public List<Transform> cameraPositions = new List<Transform>();
    int idx = 0;
    public float camSpeed;
    public AnimationCurve movementCurve;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public void NextCameraPosition()
    {
        idx++;
        StartCoroutine(MoveCamera(new Vector3(
            cameraPositions[idx].position.x,
            cameraPositions[idx].position.y,
            -10)));
    }

    public void PrevCameraPosition()
    {
        idx--;
        StartCoroutine(MoveCamera(new Vector3(
            cameraPositions[idx].position.x,
            cameraPositions[idx].position.y,
            -10)));
    }
    private IEnumerator MoveCamera(Vector3 endPos)
    {
        Vector3 startPos = transform.localPosition;

        float tElapsed = 0;
        while (tElapsed < camSpeed)
        {
            tElapsed += Time.deltaTime;
            float t = tElapsed / camSpeed;
            float tCurve = movementCurve.Evaluate(t);

            transform.localPosition = Vector3.Lerp(startPos, endPos, tCurve);

            yield return null;
        }

        transform.localPosition = endPos;
    }
}
