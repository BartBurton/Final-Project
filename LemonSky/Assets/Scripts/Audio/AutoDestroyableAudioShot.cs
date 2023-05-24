using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyableAudioShot : MonoBehaviour
{
    public float LifeTime = 1.5f;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
