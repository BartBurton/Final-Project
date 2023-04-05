using System;
using UnityEngine;
using Unity.Netcode;

public class GameWaitingToStartUI : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerChanged;
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            if (GameManager.Instance.IsWaitingToStart())
                Show();
            else
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
}
