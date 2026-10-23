using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    [SerializeField] KeyCode nextCheckpointKeybind;
    [SerializeField] KeyCode previousCheckpointKeybind;
    public List<GameObject> listOfCheckpoints = new List<GameObject>();
    CameraMover camMover;
    GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camMover = GetComponent<CameraMover>();
        //listOfCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").ToList();
        //listOfCheckpoints.Sort((a, b) => a.GetComponent<Checkpoint>().checkPointIndx.CompareTo(b.GetComponent<Checkpoint>().checkPointIndx));
    }

    private void Update()
    {
        if(Input.GetKeyDown(nextCheckpointKeybind)) NextCheckpoint();
        if (Input.GetKeyDown(previousCheckpointKeybind)) PreviousCheckpoint();
        //ToggleInvincibility
    }

    void NextCheckpoint()
    {
        player.transform.position = listOfCheckpoints[camMover.currentCamCheckpoint].transform.position;
        camMover.NextCameraPosition();
    }

    void PreviousCheckpoint()
    {
        camMover.PrevCameraPosition();
        player.transform.position = listOfCheckpoints[camMover.currentCamCheckpoint -1].transform.position;
    }
}
