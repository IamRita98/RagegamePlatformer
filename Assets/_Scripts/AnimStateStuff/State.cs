using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class State : MonoBehaviour
{

    [Header("Defaults (do not edit)")]
    public StateMachine sm;
    public Rigidbody2D rb;
    public PlayerController pc;
    public Animator upAnim;
    public Animator downAnim;
    public SpriteRenderer downSr;
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

    public void DeclareState(StateMachine _sm, Rigidbody2D _rb, PlayerController _pc, Animator _upAnim, Animator _downAnim, SpriteRenderer _downSr)
    {
        sm = _sm;
        rb = _rb;
        pc = _pc;
        upAnim = _upAnim;
        downAnim = _downAnim;
        downSr = _downSr;
    }

    public void InitializeState()
    {
        isComplete = false;
        animActive = false;
        startTime = Time.time;
    }
}
