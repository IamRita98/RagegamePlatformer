using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    static CameraMover Instance;
    public List<Transform> cameraPositions = new List<Transform>();
    public float camSpeed;
    public AnimationCurve movementCurve;
    public int currentCamCheckpoint = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public void NextCameraPosition()
    {
        currentCamCheckpoint++;
        StartCoroutine(MoveCamera(new Vector3(
            cameraPositions[currentCamCheckpoint].position.x,
            cameraPositions[currentCamCheckpoint].position.y,
            -10)));
    }

    public void PrevCameraPosition()
    {
        currentCamCheckpoint--;
        StartCoroutine(MoveCamera(new Vector3(
            cameraPositions[currentCamCheckpoint].position.x,
            cameraPositions[currentCamCheckpoint].position.y,
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
