using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class ButtonObjectDestroyer : MonoBehaviour, IButtonable
{
    [SerializeField] bool objectCanBeToggledOnAndOff = false;
    bool isActive = true;
    List<Collider2D> colls = new List<Collider2D>();
    List<SpriteRenderer> sRends = new List<SpriteRenderer>();

    private void Start()
    {
        sRends = gameObject.GetComponentsInChildren<SpriteRenderer>().ToList();
        colls = gameObject.GetComponentsInChildren<Collider2D>().ToList();
    }

    public void OnButtonInteraction()
    {
        if (isActive)
        {
            foreach (SpriteRenderer sRend in sRends) sRend.enabled = false;
            foreach (Collider2D coll in colls) coll.enabled = false;
            isActive = false;
        }
        else if (!isActive && objectCanBeToggledOnAndOff)
        {
            foreach (SpriteRenderer sRend in sRends) sRend.enabled = true;
            foreach (Collider2D coll in colls) coll.enabled = true;
            isActive = true;
        }
    }

    public void OnButtonReleased()
    {
        throw new System.NotImplementedException();
    }
}
