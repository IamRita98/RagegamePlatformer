using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField] GameObject objectToInteractWithOnButtonPress;
    [SerializeField] bool canBeShot;
    [SerializeField] bool pressurePlate;

    [Header("Animation")]
    public Sprite idleSprite;
    public Sprite pressedSprite;
    bool pressedAnim;
    SpriteRenderer sr;

    [Header("Button Audio")]
    [SerializeField] private AudioSource buttonAudio;


    int objectsOnButton = 0;


    public string additionalTagCheck;


    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        if (buttonAudio == null)
        {
            buttonAudio = GetComponent<AudioSource>();
        }


        if (buttonAudio != null)
        {
            buttonAudio.playOnAwake = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") &&
            !collision.CompareTag("Bullet") &&
            !collision.CompareTag("Block") &&
            (additionalTagCheck != null &&
             !collision.CompareTag(additionalTagCheck)))
            return;


        if (!canBeShot && collision.CompareTag("Bullet"))
            return;


        if (!pressurePlate &&
            (collision.CompareTag("Player") ||
             collision.CompareTag("Block")))
            return;

        if (pressurePlate)
        {
            sr.sprite = pressedSprite;
        }
        else
        {
            StartCoroutine(ButtonPressAnim());
        }

        objectsOnButton++;




        // Play button press sound
        if (buttonAudio != null)
        {
            buttonAudio.Play();
        }


        if (objectToInteractWithOnButtonPress != null)
        {
            objectToInteractWithOnButtonPress
                .GetComponent<IButtonable>()
                ?.OnButtonInteraction();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") &&
            !collision.CompareTag("Bullet") &&
            !collision.CompareTag("Block") &&
            (additionalTagCheck != null &&
             !collision.CompareTag(additionalTagCheck)))
            return;


        if (!canBeShot && collision.CompareTag("Bullet"))
            return;


        if (!pressurePlate &&
            (collision.CompareTag("Player") ||
             collision.CompareTag("Block")))
            return;


        objectsOnButton--;

        if (pressurePlate)
        {
            sr.sprite = idleSprite;
        }

        if (objectsOnButton > 0)
            return;


        objectsOnButton = 0;


        if (objectToInteractWithOnButtonPress != null)
        {
            objectToInteractWithOnButtonPress
                .GetComponent<IButtonable>()
                ?.OnButtonReleased();
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
