using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClingState : State
{
    [Header("StateSpecific")]
    [Header("Animations")]
    public AnimationClip wallCling;
    public AnimationClip wallClingShoot;


    public override void Enter()
    {
        upSr.enabled = false;
        downSr.enabled = false;
        wholeSr.enabled = true;

        pc.isFacingRight = pc.wallDirection == 1 ? false : true;
        pc.FlipSrX();

        pc.rb.gravityScale = 0;
        pc.rb.velocity = Vector2.zero;

        wholeAnim.Play(wallCling.name);
    }
    public override void Do()
    {
        //if (Input.GetAxis("Jump") > 0 && pc.jumpPressed)
        //{
        //    isComplete = true;
        //    pc.FlipSrX();
        //}

    }
    public override void Exit()
    {
        pc.rb.gravityScale = 1;

        upSr.enabled = true;
        downSr.enabled = true;
        wholeSr.enabled = false;


    }
}
