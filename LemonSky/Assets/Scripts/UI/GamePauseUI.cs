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
        NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) => { Debug.Log(clientId + " - disconnected"); };
    }

    void Start()
    {
        Hide();
        LocalUIManager.Instance.OnStateChanged += LocalUIStateChanged;
    }

    void Show()
    {
        Cursor.visible = true;
        gameObject.SetActive(true);
    }
    void Hide()
    {
        Cursor.visible = false;
        gameObject.SetActive(false);
    }

    void LocalUIStateChanged(LocalUIManager.UIState uIState)
    {
        if(uIState == LocalUIManager.UIState.Paused) Show(); 
        else Hide();
    }
}
