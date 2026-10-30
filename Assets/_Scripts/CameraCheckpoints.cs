using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCheckpoints : MonoBehaviour
{
    bool checkpointPassed = false;
    CameraMover camMover;
    [SerializeField] Vector2 forwardDir;
    [SerializeField] float pushDist;
    [SerializeField] float pushDuration;

    private void Start()
    {
        camMover = Object.FindAnyObjectByType<CameraMover>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (StateMachine.Instance.isDead) return;
        Vector2 dir = forwardDir;
        if (checkpointPassed == false)
        {
            camMover.NextCameraPosition();
            checkpointPassed = true;
        }
        else if (checkpointPassed == true)
        {
            camMover.PrevCameraPosition();
            checkpointPassed = false;
            dir = -dir;
        }
        StartCoroutine(MovePlayer(collision.attachedRigidbody, dir));
    }

    IEnumerator MovePlayer(Rigidbody2D rb, Vector2 dir)
    {
        Vector2 start = rb.position;
        Vector2 end = start + dir * pushDist;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.fixedDeltaTime / pushDuration;
            rb.MovePosition(Vector2.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t)));
            yield return new WaitForFixedUpdate();
        }
    }
}
