using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : State
{
    [Header("StateSpecific")]
    [Header("Animations")]
    public AnimationClip inAirUp;
    public AnimationClip inAirDown;

    [Header("For velocity range for air animation...")]
    public float velRange;

    public override void Enter()
    {

    }
    public override void Do()
    {
        float t = Utils.Map(rb.velocity.y, velRange, -velRange / 2, 0, 0.9f, true);
        upAnim.Play(inAirUp.name, 0, t);
        downAnim.Play(inAirDown.name, 0, t);

        upAnim.speed = 0;
        downAnim.speed = 0;

        if (pc.isGrounded)
        {
            isComplete = true;
        }
    }
    public override void Exit()
    {
        if (pc.isGrounded)
        {
            sm.currentState = sm.landState;
        }
        upAnim.speed = 1;
        downAnim.speed = 1;
    }
}
