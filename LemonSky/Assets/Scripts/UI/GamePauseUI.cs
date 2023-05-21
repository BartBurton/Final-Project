using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] Button returnButton;
    [SerializeField] Button exitButton;

    private bool isShow = false;

    private void Awake()
    {
        returnButton.onClick.AddListener(() =>
        {
            LocalUIManager.Instance.CurrentUIState = LocalUIManager.UIState.Default;
        });
        exitButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    void Start()
    {
        Hide();
        LocalUIManager.Instance.OnStateChanged += LocalUIStateChanged;
        NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) => { Debug.Log(clientId + " - disconnected"); };
    }

    private void FixedUpdate()
    {
        if (isShow)
        {
            Cursor.visible = true;
        }
    }

    void Show()
    {
        isShow = true;
        gameObject.SetActive(isShow);
    }
    void Hide()
    {
        isShow = false;
        gameObject.SetActive(isShow);
    }

    void LocalUIStateChanged(LocalUIManager.UIState uIState)
    {
        if (uIState == LocalUIManager.UIState.Paused) Show();
        else Hide();
    }
}
