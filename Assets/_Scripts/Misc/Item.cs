using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [Header("ObjectToInteractWith")]
    public GameObject objectToInteractWithOnButtonPress;


    [Header("Idle")]
    public AnimationCurve easing;
    public float hoverSpeed;
    float elapsed;
    Vector3 startPos;
    public Vector3 endPos;
    bool isIdle = true;
    bool moveDir;

    [Header("Pickup")]
    public float pickupVel;
    SpriteRenderer sr;
    Rigidbody2D rb;
    public float fadeTime;
    bool pickedUp;

    Color startColor;
    public Color fadeColor;
    private void Awake()
    {
        startPos = transform.localPosition;
        sr = GetComponent<SpriteRenderer>();
        startColor = sr.color;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        elapsed += Time.deltaTime * hoverSpeed;

        if (isIdle)
        {
            float elapsedHover = elapsed * hoverSpeed;
            float targetY = moveDir ? startPos.y : endPos.y;

            float progress = Mathf.PingPong(elapsedHover, 1);
            float easedProgress = easing.Evaluate(progress);

            float y = Mathf.Lerp(startPos.y, endPos.y, easedProgress);

            Vector3 newPos = transform.localPosition;
            newPos.y = y;
            transform.localPosition = newPos;
        }
        else
        {
            float t = elapsed / fadeTime;
            sr.color = Color.Lerp(startColor, fadeColor, t);
            if(elapsed >= fadeTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !pickedUp)
        {
            rb.gravityScale = 10;
            Vector3 vel = new Vector3(pickupVel * 0.1f, pickupVel, 0);
            rb.AddForce(vel, ForceMode2D.Impulse);
            isIdle = false;
            elapsed = 0;
            pickedUp = true;

            if (objectToInteractWithOnButtonPress != null)
            {
                objectToInteractWithOnButtonPress
                    .GetComponent<IButtonable>()
                    ?.OnButtonInteraction();
            }
        }
    }

}
