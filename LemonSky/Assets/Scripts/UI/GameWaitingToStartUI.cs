using System;
using UnityEngine;
using Unity.Netcode;

public class GameWaitingToStartUI : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerChanged;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            if (!GameManager.Instance.IsWaitingToStart())
                Hide();
        };
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
            Hide();
        }
    }
    void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsWaitingToStart())
        {
            Hide();
        }
    }
}
