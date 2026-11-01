using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeToRoom2 : MonoBehaviour
{
    public float timeToWaitForSceneTransition;
    [SerializeField] GameObject movingPlatform;
    [SerializeField] GameObject kilZone;
    Vector3 movingPlatStartPos;
    Collider2D coll;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        movingPlatStartPos = movingPlatform.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (movingPlatform.transform.position == movingPlatStartPos) return;
        kilZone.SetActive(false);
        StartCoroutine(WaitForSceneChange());
    }

    private IEnumerator WaitForSceneChange()
    {
        yield return new WaitForSeconds(timeToWaitForSceneTransition);
        SceneManager.LoadScene("Room2");
    }
}
