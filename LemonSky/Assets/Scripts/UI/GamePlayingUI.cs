using UnityEngine;
using System;
using Unity.Netcode;

public class GamePlayingUI : MonoBehaviour
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
        if (uIState == LocalUIManager.UIState.GamePlay)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
