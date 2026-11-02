using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEAnimatePlayer : CutsceneEvent
{
    public AudioClip sfx;
    public AudioSource audioSource;

    public Animator topHalf;
    public Animator downHalf;
    public Animator whole;

    public AnimationClip topClip;
    public AnimationClip downClip;
    public AnimationClip wholeClip;
    public override void Enter()
    {
        if (audioSource != null)
        {
            audioSource.clip = sfx;
            audioSource.Play();
        }
        if (topClip != null) topHalf.Play(topClip.name);
        if (downClip != null) downHalf.Play(downClip.name);
        if (wholeClip != null) whole.Play(wholeClip.name);
    }
}
