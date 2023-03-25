using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadText : MonoBehaviour
{
    Transform MainCamera;
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(MainCamera);
        transform.rotation = MainCamera.transform.rotation;
    }
}
