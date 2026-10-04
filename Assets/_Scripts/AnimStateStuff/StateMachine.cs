using System.Collections;
using System.Collections.Generic;
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
        if (!pc.isGrounded)
        {
            currentState = inAirState;
        }


        UpdateState();
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




}
