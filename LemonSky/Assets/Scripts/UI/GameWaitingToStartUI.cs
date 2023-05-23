using System;
using UnityEngine;
using Unity.Netcode;

public class GameWaitingToStartUI : MonoBehaviour
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
        if (uIState == LocalUIManager.UIState.WaitingToStart)
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
    //    if (GameManager.Instance.IsLocalPlayerReady())
    //    {
    //        Hide();
    //    }
    //}
    //void GameManager_OnStateChanged(object sender, EventArgs e)
    //{
    //    if (!GameManager.Instance.IsWaitingToStart())
    //    {
    //        Hide();
    //    }
    //}
}
