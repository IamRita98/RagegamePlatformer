using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public bool isTriggerable;

    public List<CutsceneEvent> events;
    int eventPos = 0;
    CutsceneEvent currentEvent;
    CutsceneEvent lastEvent;
    float startEventTime;


    private void Awake()
    {
        //StartCutscene();
    }
    private void Update()
    {
        if(currentEvent != null)
        {
            lastEvent = events[eventPos];
            currentEvent.Do();
            UpdateEvent();
        }
    }

    void UpdateEvent()
    {
        if(Time.time >= currentEvent.duration + startEventTime && eventPos < events.Count - 1)
        {
            eventPos++;
            currentEvent = events[eventPos];
            currentEvent.Enter();
            startEventTime = Time.time;
            lastEvent.Exit();
        }
    }

    public void StartCutscene()
    {
        currentEvent = events[0];
        currentEvent.Enter();
        startEventTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isTriggerable)
        {
            StartCutscene();
        }
    }
}
