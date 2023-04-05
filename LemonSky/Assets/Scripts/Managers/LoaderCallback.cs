using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    float delay = 0;
    void Start(){
        delay = new System.Random().Next(1000, 3000) / 1000;
    }
    void Update()
    {
        delay -= Time.deltaTime;
        if(delay <= 0f) Loader.LoaderCallback();
    }
}
