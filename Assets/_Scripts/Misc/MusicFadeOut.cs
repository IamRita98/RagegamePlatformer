using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFadeOut : MonoBehaviour
{
    AudioSource musicPlayer;
    bool musicPlaying = true;
    [SerializeField] float fadeDur = 1.5f;
    float maxVol;
    Coroutine fadeRoutine;

    private void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        maxVol = musicPlayer.volume;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        musicPlaying = !musicPlaying;
        if(fadeRoutine != null) if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeMus(musicPlayer.volume, musicPlaying ? maxVol : 0f));
    }

    IEnumerator FadeMus(float startVolp, float endVol)
    {
        float timer = 0f;
        while (timer < fadeDur)
        {
            timer += Time.unscaledDeltaTime;
            musicPlayer.volume = Mathf.Lerp(startVolp, endVol, timer / fadeDur);
            yield return null;
        }

        musicPlayer.volume = endVol;
        fadeRoutine = null;
    }
}
