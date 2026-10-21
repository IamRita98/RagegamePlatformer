using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCheckpoints : MonoBehaviour
{
    bool checkpointPassed = false;
    CameraMover camMover;

    private void Start()
    {
        camMover = Object.FindAnyObjectByType<CameraMover>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (checkpointPassed == false)
        {
            camMover.NextCameraPosition();
            checkpointPassed = true;
        }
        else if (checkpointPassed == true)
        {
            camMover.PrevCameraPosition();
            checkpointPassed = false;
        }
        
    }
}
