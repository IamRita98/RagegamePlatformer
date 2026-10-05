using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{
    [Header("StateSpecific")]
    [Header("Animations")]
    public AnimationClip runUp;
    public AnimationClip runDown;

    public override void Enter()
    {
        upAnim.Play(runUp.name);
        downAnim.Play(runDown.name);
    }
    public override void Do()
    {
        if (pc.horizontalMovement == 0)
        {
            isComplete = true;
        }
    }
    public override void Exit()
    {
        if (pc.isGrounded)
        {
            sm.currentState = sm.idleState;
        }
    }
}
