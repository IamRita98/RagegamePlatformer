using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEDisablePControls : CutsceneEvent
{
    public override void Enter()
    {
        StateMachine sm = Object.FindAnyObjectByType<StateMachine>();
        sm.currentState = sm.deathState;
    }

}
