using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : State
{
    [Header("StateSpecific")]
    [Header("Animations")]
    public AnimationClip landing;

    private float timePassed;

    public override void Enter()
    {
        sm.StopCoroutine("ExitShooting");

        downSr.enabled = false;
        upSr.enabled = false;
        wholeSr.enabled = true;

        wholeAnim.Play(landing.name,0,0f);
        timePassed = 0;
    }
    public override void Do()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= landing.length)
        {
            isComplete = true;
        }
    }
    public override void Exit()
    {
        downSr.enabled = true;
        upSr.enabled = true;
        wholeSr.enabled = false;
    }
}
