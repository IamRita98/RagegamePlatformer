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
        listOfCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").ToList();
        listOfCheckpoints.OrderByDescending(x => x.name);
        listOfCheckpoints.Reverse(); //Ok this is rly jank, as of my own test scene it does work but I'm not sure it will if people rename or put it in a diff order
    }

    private void Update()
    {
        if(Input.GetKeyDown(nextCheckpointKeybind)) NextCheckpoint();
        if (Input.GetKeyDown(previousCheckpointKeybind)) PreviousCheckpoint();
        //ToggleInvincibility
    }

    void NextCheckpoint()
    {
        camMover.NextCameraPosition();
        player.transform.position = listOfCheckpoints[camMover.currentCamCheckpoint -1].transform.position;
    }

    void PreviousCheckpoint()
    {
        camMover.PrevCameraPosition();
        player.transform.position = listOfCheckpoints[camMover.currentCamCheckpoint -1].transform.position;
    }
}
