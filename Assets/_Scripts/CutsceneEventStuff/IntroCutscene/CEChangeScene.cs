using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CEChangeScene : CutsceneEvent
{
    public string sceneName;
    public override void Enter()
    {
        SceneManager.LoadScene(sceneName);
    }
}
