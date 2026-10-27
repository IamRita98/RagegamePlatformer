using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField] GameObject objectToInteractWithOnButtonPress;
    [SerializeField] bool canBeShot;
    [SerializeField] bool pressurePlate;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet") && !collision.CompareTag("Block")) return;

        if (!canBeShot && collision.CompareTag("Bullet")) return;

        if (!pressurePlate && (collision.CompareTag("Player") || collision.CompareTag("Block"))) return;

        objectToInteractWithOnButtonPress.GetComponent<IButtonable>().OnButtonInteraction();
    }
}
