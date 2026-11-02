using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnabler : MonoBehaviour
{
    public List<AudioSource> audioSources;
    public bool audioOn = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (!audioOn)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.enabled = true;
            }

            audioOn = true;
        }
        else
        {
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.enabled = false;
            }
            audioOn = false;
        }
    }
}
