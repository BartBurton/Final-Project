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

    void LocalUI_OnStateChanged(LocalUIManager.UIState prev, LocalUIManager.UIState next)
    {
        if (next == LocalUIManager.UIState.WaitingPlayers)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
