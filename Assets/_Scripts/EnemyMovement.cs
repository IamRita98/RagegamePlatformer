using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private int startingDirection = 1;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public GameObject deathEffect;

    [Header("Death Audio")]
    [SerializeField] private AudioSource deathAudio;

    // Whether the enemy can be killed by stomping or shooting
    public bool isStompable;
    public bool isShootable;
    public bool walksOff;

    public enum FloorDirection
    {
        down,
        up,
        left,
        right
    }

    public FloorDirection floorDirection;

    [Header("Proximity Spike Trap")]
    public bool hasProximitySpike;

    [SerializeField] private GameObject spikeGO;
    [SerializeField] private Transform spikeCheck;
    [SerializeField] private Vector2 spikeCheckSize = new Vector2(1.5f, 2f);
    [SerializeField] private Vector3 spikeEndPosition;
    [SerializeField] private float spikeMoveTime = 0.2f;
    [SerializeField] private AnimationCurve spikeMovementCurve;

    private Vector3 spikeStartPosition;
    private bool spikeActivated;

    private float wallCheckDistance = 1f;
    private bool isTouchingWall;
    private bool isGroundAhead;
    private int currentDirection;
    private Vector3 spawnPosition;

    private Collider2D[] enemyColliders;
    private Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    private Vector2 moveAxis;
    private Vector2 groundCheckBase;
    private Vector2 floorDir = Vector2.down;

    [HideInInspector] public bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        currentDirection = startingDirection;
        groundCheckBase = groundCheck.localPosition;

        switch (floorDirection)
        {
            case FloorDirection.down:
                floorDir = Vector2.down;
                break;

            case FloorDirection.up:
                floorDir = Vector2.up;
                break;

            case FloorDirection.left:
                floorDir = Vector2.left;
                break;

            case FloorDirection.right:
                floorDir = Vector2.right;
                break;
        }

        GroundCheckPosUpdate();

        spriteRenderer.flipX = currentDirection == 1;

        if (hasProximitySpike && spikeGO != null)
        {
            spikeStartPosition = spikeGO.transform.localPosition;
        }
        else if (spikeGO != null)
        {
            spikeGO.SetActive(false);
        }

        spawnPosition = transform.position;
        enemyColliders = GetComponentsInChildren<Collider2D>();
    }

    private void Start()
    {
        StateMachine.Instance.onPlayerRespawn.AddListener(Respawn);

        if (floorDirection != FloorDirection.down)
        {
            rb.gravityScale = 0f;
        }

        moveAxis = new Vector2(-floorDir.y, floorDir.x);
    }

    private void OnDestroy()
    {
        if (StateMachine.Instance != null)
        {
            StateMachine.Instance.onPlayerRespawn.RemoveListener(Respawn);
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        CheckForWall();
        CheckForFloor();

        bool lavaAhead = CheckForLava();

        if (isTouchingWall || lavaAhead || (!walksOff && !isGroundAhead))
        {
            ChangeDirection();
        }

        if (hasProximitySpike && !spikeActivated)
        {
            CheckForPlayerAbove();
        }

        if (floorDir == Vector2.down)
        {
            rb.velocity = new Vector2(
                currentDirection * movementSpeed,
                rb.velocity.y
            );
        }
        else
        {
            rb.velocity = moveAxis * (currentDirection * movementSpeed);
        }
    }

    private void GroundCheckPosUpdate()
    {
        Vector2 pos = groundCheckBase;

        if (Mathf.Abs(moveAxis.x) > 0.5f)
        {
            pos.x = Mathf.Abs(groundCheckBase.x)
                    * currentDirection
                    * Mathf.Sign(moveAxis.x);
        }
        else
        {
            pos.y = Mathf.Abs(groundCheckBase.y)
                    * currentDirection
                    * Mathf.Sign(moveAxis.y);
        }

        groundCheck.localPosition = pos;
    }

    private IEnumerator RaiseSpike()
    {
        spikeActivated = true;

        Vector3 startPos = spikeGO.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < spikeMoveTime)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / spikeMoveTime;
            float curve = spikeMovementCurve.Evaluate(t);

            spikeGO.transform.localPosition = Vector3.Lerp(
                startPos,
                spikeEndPosition,
                curve
            );

            yield return null;
        }

        spikeGO.transform.localPosition = spikeEndPosition;
    }

    private void CheckForPlayerAbove()
    {
        if (spikeCheck == null)
            return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            spikeCheck.position,
            spikeCheckSize,
            0f
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                StartCoroutine(RaiseSpike());
                return;
            }
        }
    }

    private void CheckForWall()
    {
        Vector2 direction = currentDirection * moveAxis;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            wallCheckDistance,
            LayerMask.GetMask("Ground")
        );

        isTouchingWall = hit.collider != null;

        Debug.DrawRay(
            transform.position,
            direction * wallCheckDistance,
            Color.red
        );
    }

    private void CheckForFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            floorDir,
            wallCheckDistance,
            groundLayer
        );

        isGroundAhead = hit.collider != null;

        Debug.DrawRay(
            groundCheck.position,
            floorDir * wallCheckDistance,
            Color.green
        );
    }

    private bool CheckForLava()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            groundCheck.position,
            floorDir,
            wallCheckDistance
        );

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Lava"))
                return true;
        }

        return false;
    }

    private void ChangeDirection()
    {
        currentDirection *= -1;

        spriteRenderer.flipX = currentDirection == 1;

        GroundCheckPosUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy touches another enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ChangeDirection();
            return;
        }

        // Player touches enemy body
        PlayerController player =
            collision.gameObject.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.Die();
        }
    }

    public void Stomp(PlayerController player)
    {
        if (!isStompable)
        {
            player.Die();
            return;
        }

        player.Bounce();
        Die();
    }

    public void ActivateSpike()
    {
        if (hasProximitySpike && !spikeActivated)
        {
            StartCoroutine(RaiseSpike());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasProximitySpike &&
            !spikeActivated &&
            collision.CompareTag("Player"))
        {
            StartCoroutine(RaiseSpike());
        }
    }

    public void Die()
    {
        // Prevent death effects and sound from playing more than once
        if (isDead)
            return;

        isDead = true;

        // Play enemy death sound
        if (deathAudio != null)
        {
            deathAudio.Play();
        }

        // Camera shake
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.StartCoroutine(
                CameraShake.Instance.Shake(0.25f, 0.8f)
            );
        }

        // Spawn death effect
        if (deathEffect != null)
        {
            Instantiate(
                deathEffect,
                transform.position,
                Quaternion.identity
            );
        }

        // Disable spike object
        if (spikeGO != null)
        {
            spikeGO.SetActive(false);
        }

        // Stop enemy movement
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        // Hide enemy sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // Disable enemy colliders
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

        rb.simulated = true;
        rb.velocity = Vector2.zero;

        foreach (Collider2D col in enemyColliders)
        {
            col.enabled = true;
        }

        GroundCheckPosUpdate();

        // Reset spike trap
        if (hasProximitySpike && spikeGO != null)
        {
            StopAllCoroutines();

            spikeActivated = false;
            spikeGO.transform.localPosition = spikeStartPosition;
            spikeGO.SetActive(true);
        }
    }
}