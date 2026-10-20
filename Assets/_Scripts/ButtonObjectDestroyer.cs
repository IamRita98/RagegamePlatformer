using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObjectDestroyer : MonoBehaviour, IButtonable
{
    [SerializeField] bool objectCanBeToggledOnAndOff = false;
    bool isActive = true;
    Collider2D coll;
    SpriteRenderer sRend;

    private void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    public void OnButtonInteraction()
    {
        if (isActive)
        {
            sRend.enabled = false;
            coll.enabled = false;
            isActive = false;
        }
        else if (!isActive && objectCanBeToggledOnAndOff)
        {
            sRend.enabled = true;
            coll.enabled = true;
            isActive = true;
        }
    }
}
