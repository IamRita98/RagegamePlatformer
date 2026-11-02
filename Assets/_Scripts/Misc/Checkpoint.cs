using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public int checkPointIndx;
    public CameraMover camMover;

    private void Start()
    {
        camMover = FindAnyObjectByType<CameraMover>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StateMachine smRef = col.gameObject.GetComponentInChildren<StateMachine>();
            smRef.respawnPos = transform.position;

            PlayerPrefs.SetFloat("CheckpointXPos", transform.position.x);
            PlayerPrefs.SetFloat("CheckpointYPos", transform.position.y);
            PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetInt("HasCheckpoint", 1);
            PlayerPrefs.SetInt("SavedCameraIndex", camMover.currentCamCheckpoint);
            PlayerPrefs.SetInt("CurrentLifeCount", UILivesManager.lives);

            PlayerPrefs.Save();
        }
    }
}
