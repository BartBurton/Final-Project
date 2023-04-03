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
            GameInputs.Instance.IsPaused = false;
        });
        exitButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    void Start()
    {
        Hide();
        GameInputs.Instance.OnPauseAction += GameInputs_OnStateChanged;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
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

    void GameInputs_OnStateChanged(object sender, EventArgs e)
    {
        if (GameInputs.Instance.IsPaused)
            Show();
        else
            Hide();
    }
    void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying()) return;
        GameInputs.Instance.IsPaused = false;
    }

}
