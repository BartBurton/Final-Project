using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    void Start()
    {
        Hide();
        LocalUIManager.Instance.OnStateChanged += LocalUIStateChanged;
    }

    void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }

    void LocalUIStateChanged(LocalUIManager.UIState uIState)
    {
        if (uIState == LocalUIManager.UIState.Paused)
            Show();
        else
            Hide();
    }
}
