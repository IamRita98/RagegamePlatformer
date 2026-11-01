using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnabler : MonoBehaviour
{
    public AudioSource button1;
    public AudioSource button2;
    public static bool audioOn = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (!audioOn)
        {
            button1.enabled = true;
            button2.enabled = true;
            audioOn = true;
        }
        else
        {
            button1.enabled = false;
            button2.enabled = false;
            audioOn = false;
        }
    }
}
