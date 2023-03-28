using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameWaitingPlayersUI : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerChanged;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }

    void GameManager_OnLocalPlayerChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsLocalPlayerReady())
        {
            Show();
        }
    }
    void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsWaitingToStart() && GameManager.Instance.IsLocalPlayerReady())
            Show();
        else
            Hide();
    }
}
