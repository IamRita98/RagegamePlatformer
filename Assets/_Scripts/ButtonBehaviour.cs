using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField]GameObject objectToInteractWithOnButtonPress;
    [SerializeField] bool canBeShot;
    [SerializeField] bool canBePlayerInteracted;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet")) return;
        if (!canBeShot && collision.CompareTag("Bullet")) return;
        if (!canBePlayerInteracted && collision.CompareTag("Player")) return;
        objectToInteractWithOnButtonPress.GetComponent<IButtonable>().OnButtonInteraction();
    }
}
