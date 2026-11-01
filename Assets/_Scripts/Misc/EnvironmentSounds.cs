using UnityEngine;


public class EnvironmentSounds : MonoBehaviour
{
    [Header("Surface Sound")]
    [SerializeField] private AudioSource surfaceAudio;


    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";


    private int playerContacts = 0;


    private void Awake()
    {
        if (surfaceAudio == null)
        {
            surfaceAudio = GetComponent<AudioSource>();
        }


        if (surfaceAudio != null)
        {
            surfaceAudio.playOnAwake = false;
            surfaceAudio.loop = true;
            surfaceAudio.Stop();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            StartSurfaceSound();
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            StopSurfaceSound();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            StartSurfaceSound();
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            StopSurfaceSound();
        }
    }


    private void StartSurfaceSound()
    {
        playerContacts++;


        if (surfaceAudio != null && !surfaceAudio.isPlaying)
        {
            surfaceAudio.Play();
        }
    }


    private void StopSurfaceSound()
    {
        playerContacts = Mathf.Max(0, playerContacts - 1);


        if (playerContacts == 0 && surfaceAudio != null)
        {
            surfaceAudio.Stop();
        }
    }
}
