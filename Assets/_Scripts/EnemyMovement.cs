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

    // Whether or not this enemy is killed when jumped on, shot at, both, or neither
    public bool isStompable;
    public bool isShootable;
    public bool walksOff;

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
    Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [HideInInspector] public bool isDead;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        currentDirection = startingDirection;

        groundCheck.localPosition = new Vector2(
            Mathf.Abs(groundCheck.localPosition.x) * currentDirection,
            groundCheck.localPosition.y
        );

        spriteRenderer.flipX = currentDirection == 1;

        if (hasProximitySpike && spikeGO != null)
        {
            spikeStartPosition = spikeGO.transform.localPosition;
        }
    }

    private void FixedUpdate()
    {
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

        rb.velocity = new Vector2(
            currentDirection * movementSpeed,
            rb.velocity.y
        );
    }

    IEnumerator RaiseSpike()
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

    void CheckForPlayerAbove()
    {
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

    // I stole this from PlayerController script
    void CheckForWall()
    {
        Vector2 direction = currentDirection == 1
            ? Vector2.right
            : Vector2.left;

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

    void CheckForFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            wallCheckDistance,
            groundLayer
        );

        isGroundAhead = hit.collider != null;

        Debug.DrawRay(
            groundCheck.position,
            Vector2.down * wallCheckDistance,
            Color.green
        );
    }

    bool CheckForLava()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            groundCheck.position,
            Vector2.down,
            wallCheckDistance
        );

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Lava"))
                return true;
        }

        return false;
    }

    void ChangeDirection()
    {
        currentDirection *= -1;

        spriteRenderer.flipX = currentDirection == 1;

        groundCheck.localPosition = new Vector2(
            Mathf.Abs(groundCheck.localPosition.x) * currentDirection,
            groundCheck.localPosition.y
        );
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
        PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();

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
        else
        {
            player.Bounce();
            Die();
        }
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
        if (hasProximitySpike && !spikeActivated && collision.CompareTag("Player"))
        {
            StartCoroutine(RaiseSpike());
        }
    }

    public void Die()
    {
        CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(0.25f, 0.8f));
        isDead = true;
        if(deathEffect != null)
        {
            GameObject newDE = Instantiate(deathEffect, transform.position, Quaternion.identity);
            SpriteRenderer desr = newDE.GetComponent<SpriteRenderer>();
            desr.flipX = spriteRenderer.flipX ? true : false;
        }
        gameObject.SetActive(false);
    }
}
