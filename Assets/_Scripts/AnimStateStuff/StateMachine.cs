using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class StateMachine : MonoBehaviour
{
    [Header("Player Controller")]
    public PlayerController pc;

    [Header("Respawning")]
    public float respawnDelay;
    public Vector2 respawnPos;

    [Header("States Holder GameObject")]
    public GameObject behaviors;

    [Header("Animation")]
    public SpriteRenderer upSr;
    public SpriteRenderer downSr;
    public SpriteRenderer wholeSr;
    public Animator upAnim;
    public Animator downAnim;
    public Animator wholeAnim;

    [Header("States")]

    public TextMeshPro stateDebugText;
    public State currentState;
    public State lastState;
    [HideInInspector]public bool isDead;

    [Header("")]
    public IdleState idleState;
    public RunState runState;
    public InAirState inAirState;
    public LandingState landState;
    public WallClingState wallClingState;
    public DeathState deathState;

    public bool shooting;

    public UnityEvent onPlayerRespawn;
    private void Awake()
    {
        State[] states = behaviors.GetComponentsInChildren<State>();

        foreach (State newState in states)
        {
            newState.DeclareState(this, pc.rb, pc, upAnim, downAnim, wholeAnim, upSr, downSr, wholeSr);
        }

        currentState = idleState;
        respawnPos = transform.position;
    }

    private void Update()
    {
        Shoot();

        currentState.Do();

        UpdateState();
    }
    void UpdateState()
    {
        lastState = currentState;

        if (isDead)
        {
            currentState = deathState;
        }
        else if (!pc.isGrounded)
        {
            if (pc.isTouchingWall)
            {
                currentState = wallClingState;
            }
            else
            {
                currentState = inAirState;
            }
        }
        else if (currentState != landState && pc.horizontalMovement != 0)
        {
            currentState = runState;
        }
        else if ((currentState == landState && currentState.isComplete))
        {
            if (pc.horizontalMovement != 0)
            {
                currentState = runState;
            }
            else
            {
                currentState = idleState;
            }
        }

        if (lastState != currentState || currentState.isComplete)
        {
            lastState.Exit();
            currentState.InitializeState();
            currentState.Enter();
        }
        stateDebugText.text = currentState.name;
    }

    void Shoot()
    {
        if (pc.gunOnCD && !shooting)
        {
            EnterShooting();
        }

        if(currentState != landState && shooting && !pc.gunOnCD)
        {
            StartCoroutine("ExitShooting");
        }
    }
    
    void EnterShooting()
    {
        StopCoroutine("ExitShooting");

        shooting = true;

        if(currentState == wallClingState)
        {
            wholeAnim.Play("WholeWallShoot",0,0f);
        }
        else
        {
            upSr.enabled = false;
            wholeSr.enabled = true;

            if (pc.aimingUp)
            {
                wholeAnim.Play("WholeShootUp", 0, 0f);
            }
            else
            {
                wholeAnim.Play("WholeShoot", 0, 0f);
            }
        }
    }

    public IEnumerator ExitShooting()
    {
        shooting = false;

        yield return new WaitForSeconds(0.1f);
        if (!isDead)
        {
            if (currentState == wallClingState)
            {
                wholeAnim.Play("WholeWallCling");
            }
            else
            {
                upSr.enabled = true;
                wholeSr.enabled = false;
            }
        }
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        pc.transform.position = respawnPos;
        isDead = false;
        currentState.ForceExit();
        currentState = idleState;

        onPlayerRespawn.Invoke();

        pc.rb.velocity = Vector2.zero;

        downSr.enabled = true;
        upSr.enabled = true;
        wholeSr.enabled = false;
    }
}
