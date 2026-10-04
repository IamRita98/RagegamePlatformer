using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [Header("Player Controller")]
    public PlayerController pc;

    [Header("States Holder GameObject")]
    public GameObject behaviors;

    [Header("Animation")]
    public SpriteRenderer upSr;
    public SpriteRenderer downSr;
    public Animator upAnim;
    public Animator downAnim;


    [Header("States")]

    public TextMeshPro stateDebugText;
    public State currentState;
    public State lastState;

    [Header("")]
    public IdleState idleState;
    public RunState runState;
    public InAirState inAirState;
    public LandingState landState;
    public WallClingState wallClingState;
    public DeathState deathState;

    private void Awake()
    {
        State[] states = behaviors.GetComponentsInChildren<State>();

        foreach (State newState in states)
        {
            newState.DeclareState(this, pc.rb, pc, upAnim, downAnim, downSr);
        }

        currentState = idleState;
    }

    private void Update()
    {
        FlipSrX(upSr);
        FlipSrX(downSr);

        if (!pc.isGrounded)
        {
            currentState = inAirState;
        }

        UpdateState();
        stateDebugText.text = currentState.name;
    }
    void UpdateState()
    {
        lastState = currentState;

        if(lastState != currentState || currentState.isComplete)
        {
            lastState.Exit();
            currentState.InitializeState();
            currentState.Enter();
        }
    }

    void FlipSrX(SpriteRenderer sr)
    {
        if (pc.isFacingRight)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
    }




}
