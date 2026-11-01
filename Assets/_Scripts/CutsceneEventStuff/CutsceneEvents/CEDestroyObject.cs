using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEDestroyObject : CutsceneEvent
{
    public GameObject goToDestroy;
    public override void Enter()
    {
        goToDestroy.SetActive(false);
    }

}
