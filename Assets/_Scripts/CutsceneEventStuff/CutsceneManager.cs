using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public List<CutsceneEvent> events;
    int eventPos = 0;
    CutsceneEvent currentEvent;
    CutsceneEvent lastEvent;

    private void Awake()
    {
        currentEvent = events[0];
        currentEvent.Enter();
    }
    private void Update()
    {
        lastEvent = events[eventPos];
        currentEvent.Do();
        UpdateEvent();
    }

    void UpdateEvent()
    {
        if(Time.time >= currentEvent.duration && eventPos < events.Count - 1)
        {
            eventPos++;
            currentEvent = events[eventPos];
            currentEvent.Enter();
            lastEvent.Exit();
        }
    }
}
