using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public List<GameObject>flightPath = new List<GameObject>();
    [SerializeField] float speed;
    [SerializeField] float lingerTimeAtPosition;
    [SerializeField] float arch;
    public bool isStompable;
    public bool isShootable;
    SpriteRenderer spriteRenderer;
    bool isDead = false;
    private Collider2D[] enemyColliders;
    Vector2 spawnPosition;
    private int currentDirection;
    [SerializeField] private int startingDirection = 1;


    private void Start()
    {
        StartCoroutine(FlightPath());
        StateMachine.Instance.onPlayerRespawn.AddListener(Respawn);
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyColliders = GetComponentsInChildren<Collider2D>();
        spawnPosition = transform.position;
    }

    private void OnDestroy()
    {
        if (StateMachine.Instance != null)
        {
            StateMachine.Instance.onPlayerRespawn.RemoveListener(Respawn);
        }
    }

    private IEnumerator FlightPath()
    {
        int idx = 0;
        int dir = 1;
        while (true)
        {
            if (idx + dir >= flightPath.Count || idx + dir < 0) dir *= -1;

            Vector2 startPos = transform.position;
            Vector2 endPos = flightPath[idx + dir].transform.position;

            float duration = Vector2.Distance(startPos, endPos) / speed;
            float prog = 0f;

            while (prog < 1f)
            {
                prog += Time.deltaTime / Mathf.Max(duration, .0001f);
                Vector2 pos = Vector2.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, prog));
                pos.y += Mathf.Sin(Mathf.SmoothStep(0f, 1f, prog) * Mathf.PI) * arch;
                transform.position = pos;
                yield return null;
            }
            transform.position = endPos;
            idx += dir;

            if(lingerTimeAtPosition > 0f) yield return new WaitForSeconds(lingerTimeAtPosition);
        }
    }

    public void Stomp(PlayerController player)
    {
        if (!isStompable)
        {
            player.Die();
            return;
        }
        else
        {
            player.Bounce();
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.Die();
        }
    }

    public void Die()
    {
        CameraShake.Instance.StartCoroutine(
            CameraShake.Instance.Shake(0.25f, 0.8f)
        );

        isDead = true;

/*        if (deathEffect != null)
        {
            Instantiate(
                deathEffect,
                transform.position,
                Quaternion.identity
            );
        }*/

/*        rb.velocity = Vector2.zero;
        rb.simulated = false;*/

        spriteRenderer.enabled = false;

        foreach (Collider2D col in enemyColliders)
        {
            col.enabled = false;
        }
    }

    private void Respawn()
    {
        isDead = false;

        transform.position = spawnPosition;

        currentDirection = startingDirection;

        spriteRenderer.flipX = currentDirection == 1;
        spriteRenderer.enabled = true;

       /* rb.simulated = true;
        rb.velocity = Vector2.zero;*/

        foreach (Collider2D col in enemyColliders)
        {
            col.enabled = true;
        }
    }
}
