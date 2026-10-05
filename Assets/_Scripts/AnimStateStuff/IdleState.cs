using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    [Header("StateSpecific")]
    [Header("Animations")]
    public AnimationClip idleUp;
    public AnimationClip idleDown;

    public override void Enter()
    {
        Debug.Log("entered idle");
        upAnim.Play(idleUp.name);
        downAnim.Play(idleDown.name);
    }
    public override void Do()
    {

    }
    public override void Exit()
    {

    }

}
