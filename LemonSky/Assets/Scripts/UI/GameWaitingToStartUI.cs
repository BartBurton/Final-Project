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

    void LocalUI_OnStateChanged(LocalUIManager.UIState prev, LocalUIManager.UIState next)
    {
        if (next == LocalUIManager.UIState.WaitingToStart)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
