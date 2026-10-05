using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    [Header("StateSpecific")]
    [Header("Animations")]
    public AnimationClip death;
    public AnimationClip deathInAir;
    private bool diedInAir;
    public override void Enter()
    {
        downSr.enabled = false;
        upSr.enabled = false;
        wholeSr.enabled = true;

        pc.horizontalMovement = 0;

        if (pc.isGrounded)
        {
            pc.rb.velocity = Vector2.zero;
            wholeAnim.Play(death.name, 0, 0f);
        }
        else
        {
            Vector2 vel = Vector2.zero;
            vel.y = 10;
            vel.x = pc.isFacingRight ? -10 : 10;
            pc.rb.velocity = vel;

            wholeAnim.Play(deathInAir.name, 0, 0f);
            diedInAir = true;
        }

        sm.StartCoroutine("Respawn");
    }
    public override void Do()
    {
        if (diedInAir && pc.isGrounded)
        {
            wholeAnim.Play(death.name, 0, 0.25f);
            diedInAir = false;
        }
    }
    public override void Exit()
    {
        downSr.enabled = true;
        upSr.enabled = true;
        wholeSr.enabled = false;
    }
}
