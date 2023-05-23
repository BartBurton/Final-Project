using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameWaitingPlayersUI : MonoBehaviour
{
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
        if (uIState == LocalUIManager.UIState.WaitingPlayers)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    //void GameManager_OnLocalPlayerChanged(object sender, EventArgs e)
    //{
    //    Debug.Log("Ждем - " + GameManager.Instance.IsWaitingToStart());
    //    if (GameManager.Instance.IsLocalPlayerReady() && GameManager.Instance.IsWaitingToStart())
    //        Show();
    //    else
    //        Hide();
    //}
    //void GameManager_OnStateChanged(object sender, EventArgs e)
    //{
    //    if (!GameManager.Instance.IsWaitingToStart())
    //        Hide();
    //}
}
