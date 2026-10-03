using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public AnimationClip animClip;
    public Animator anim;
    public Rigidbody2D rb;
    public PlayerController pc;
    public bool isComplete { get; protected set; }
    public bool animActive;

    float startTime;

    public float time => Time.time - startTime;

    public virtual void Enter()
    {

    }
    public virtual void Do()
    {

    }
    public virtual void Exit()
    {

    }

    public void ForceExit()
    {
        isComplete = true;
    }

    public void DeclareState(Animator _anim, Rigidbody2D _rb, PlayerController _pc)
    {
        anim = _anim;
        rb = _rb;
        pc = _pc;
    }

    public void InitializeState()
    {
        isComplete = false;
        animActive = false;
        startTime = Time.time;
    }
}
