using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.IO.LowLevel.Unsafe;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private Checkpoint checkpoint;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private StateMachine stateMachine;

    private Transform lastSavedCheckpoint;

    private void Awake()
    {
        canvasGroup.interactable = true;
    }

    private void Start()
    {

    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("CheckpointXPos");
        PlayerPrefs.DeleteKey("CheckpointYPos");
        PlayerPrefs.DeleteKey("SavedScene");
        PlayerPrefs.DeleteKey("HasCheckpoint");
        PlayerPrefs.DeleteKey("ContinueGame");
        PlayerPrefs.DeleteKey("SavedCameraIndex");

        PlayerPrefs.Save();

        StartCoroutine(UIFade(canvasGroup.alpha, 0));
        cutsceneManager.StartCutscene();
        canvasGroup.interactable = false;
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.GetInt("HasCheckpoint", 0) == 1)
        {
            string savedScene = PlayerPrefs.GetString("SavedScene", "");

            if (!string.IsNullOrEmpty(savedScene))
            {
                PlayerPrefs.SetInt("ContinueGame", 1);
                PlayerPrefs.Save();

                SceneManager.LoadScene(savedScene);
            }
            else
            {
                Debug.LogWarning("No saved scene found!");
            }
        }
        else
        {
            Debug.Log("No saved checkpoint found!");
        }
    }

    public void ControlsMenuOn()
    {
        controlsMenu.SetActive(true);
    }

    public void ControlsMenuOff()
    {
        controlsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit game");
    }

    private IEnumerator UIFade(float start, float end)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}
