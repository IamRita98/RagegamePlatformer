using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StateMachine smRef = col.gameObject.GetComponentInChildren<StateMachine>();
            smRef.respawnPos = transform.position;
        }
    }
}
