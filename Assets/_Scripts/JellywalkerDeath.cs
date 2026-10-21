using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellywalkerDeath : MonoBehaviour
{
    public float destroyTime;

    private void Update()
    {
        destroyTime -= Time.deltaTime;
        if(destroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
