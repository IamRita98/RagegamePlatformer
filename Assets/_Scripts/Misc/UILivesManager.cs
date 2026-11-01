using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILivesManager : MonoBehaviour
{
    public TMP_Text lifeCount;
    public GameObject livesGO;
    static int lives = 3;

    [SerializeField]float bounceScaling;
    [SerializeField] float bounceDur;
    Vector3 baseScale;

    public TMP_Text timerText;
    static float time;


    private void Start()
    {
        baseScale = livesGO.transform.localScale;
        StateMachine.Instance.onPlayerRespawn.AddListener(PlayerDeath);
        RefreshUI();
    }

    private void OnDisable()
    {
        StateMachine.Instance.onPlayerRespawn.RemoveListener(PlayerDeath);
    }

    private void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        time += Time.deltaTime;
        int minutes = (int)(time / 60f);
        float seconds = time % 60f;
        timerText.text = $"{minutes:00}:{seconds:00.00}";
    }

    void PlayerDeath()
    {
        lives--;
        RefreshUI();
        if (livesGO.activeSelf) StartCoroutine(Bounce());
    }

    IEnumerator Bounce()
    {
        float t = 0f;
        while (t < bounceDur)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t/bounceDur);
            float s = 1f + (bounceScaling - 1) * Mathf.Sin(p * Mathf.PI);
            livesGO.transform.localScale = baseScale * s;
            yield return null;
        }

        livesGO.transform.localScale = baseScale;
    }

    void RefreshUI()
    {
        lifeCount.text = lives.ToString();
        bool inLevel1 = false;
        if (SceneManager.GetActiveScene().name == "Room1") inLevel1 = true;
        if(inLevel1 && lives <= 0) livesGO.SetActive(false);
    }
}
