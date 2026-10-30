using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField] GameObject objectToInteractWithOnButtonPress;
    [SerializeField] bool canBeShot;
    [SerializeField] bool pressurePlate;
    int objectsOnButton = 0;

    public Sprite idleSprite;
    public Sprite pressedSprite;
    bool pressedAnim;
    public string additionalTagCheck;
    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet") && !collision.CompareTag("Block") && (additionalTagCheck != null && !collision.CompareTag(additionalTagCheck))) return;

        if (!canBeShot && collision.CompareTag("Bullet")) return;

        if (!pressurePlate && (collision.CompareTag("Player") || collision.CompareTag("Block"))) return;

        if (pressurePlate)
        {
            sr.sprite = pressedSprite;
        }
        else
        {
            StartCoroutine(ButtonPressAnim());
        }
            objectsOnButton++;
        objectToInteractWithOnButtonPress.GetComponent<IButtonable>().OnButtonInteraction();

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet") && !collision.CompareTag("Block") && (additionalTagCheck != null && !collision.CompareTag(additionalTagCheck))) return;

        if (!canBeShot && collision.CompareTag("Bullet")) return;

        if (!pressurePlate && (collision.CompareTag("Player") || collision.CompareTag("Block"))) return;
        objectsOnButton--;
        if (objectsOnButton > 0) return;
        objectToInteractWithOnButtonPress.GetComponent<IButtonable>().OnButtonReleased();


        if (pressurePlate)
        {
            sr.sprite = idleSprite;
        }
    }

    private IEnumerator ButtonPressAnim()
    {
        if (pressedAnim)
        {
            yield break;
        }
        pressedAnim = true;
        sr.sprite = pressedSprite;
        yield return new WaitForSeconds(0.5f);
        sr.sprite = idleSprite;
        pressedAnim = false;
    }
}
