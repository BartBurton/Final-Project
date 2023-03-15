using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using LemonSky.Net;
using Unity.Netcode;

public class ApplicationController : LifetimeScope
{
    [SerializeField] NetworkManager NetworkManager;

    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        builder.RegisterComponent(NetworkManager);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 120;
        SceneManager.LoadScene("Scene");
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
