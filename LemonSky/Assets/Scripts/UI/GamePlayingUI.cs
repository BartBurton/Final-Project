using UnityEngine;
using System;
using Unity.Netcode;

public class GamePlayingUI : MonoBehaviour
{
    void Start()
    {
        Hide();
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    }
    void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }

    void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
            Show();
        else
            Hide();
    }
    void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        if (GameManager.Instance.IsGamePlaying())
            Show();
        else
            Hide();
    }
}
