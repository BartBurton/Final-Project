using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _returnButton.onClick.AddListener(() =>
        {
            LocalUIManager.Instance.CurrentUIState = LocalUIManager.UIState.GamePlay;
        });
        _exitButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    void Start()
    {
        Hide();
        LocalUIManager.Instance.OnStateChanged += LocalUI_OnStateChanged;
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }

    void LocalUI_OnStateChanged(LocalUIManager.UIState uIState)
    {
        if (uIState == LocalUIManager.UIState.Paused)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
