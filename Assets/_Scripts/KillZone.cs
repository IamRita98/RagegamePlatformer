using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private PlayerController pc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pc = other.GetComponent<PlayerController>();
            pc.sm.isDead = true;
        }
    }
}
