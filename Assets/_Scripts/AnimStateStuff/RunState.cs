using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{
    [Header("StateSpecific")]
    [Header("Animations")]
    public AnimationClip runUp;
    public AnimationClip runDown;

    [Header("Audio")]
    public AudioSource footstepAudio;

    public override void Enter()
    {
        upAnim.Play(runUp.name);
        downAnim.Play(runDown.name);

        // Start the walking/running sound
        if (footstepAudio != null && !footstepAudio.isPlaying)
        {
            footstepAudio.Play();
        }
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
        // Stop the sound when leaving the run state
        if (footstepAudio != null && footstepAudio.isPlaying)
        {
            footstepAudio.Stop();
        }

        if (pc.isGrounded)
        {
            sm.currentState = sm.idleState;
        }
    }
}