using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using StarterAssets;

public class GameMultiplayer : NetworkBehaviour
{
    [SerializeField] int MAX_PLAYER_AMOUNT = 4;
    public static GameMultiplayer Instance { get; private set; }

    public event EventHandler OnTryingJoinGame;
    public event EventHandler OnFailJoinGame;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = null;
        NetworkManager.Singleton.ConnectionApprovalCallback = NetworkManager_ConnectionCallback;
        NetworkManager.Singleton.StartHost();
        Loader.Load(Loader.Scene.CharacterSelect, true);
    }
    public void StartClient()
    {
        OnTryingJoinGame?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
    }
    public void StartServer()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = null;
        NetworkManager.Singleton.ConnectionApprovalCallback = NetworkManager_ConnectionCallback;
        NetworkManager.Singleton.StartServer();
        Loader.Load(Loader.Scene.CharacterSelect, true);
    }

    void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailJoinGame?.Invoke(this, EventArgs.Empty);
    }
    void NetworkManager_ConnectionCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        var approved = !(SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelect.ToString() || NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT);
        connectionApprovalResponse.Approved = approved;
        Debug.Log("Подключение " + (approved ? "одобрено" : "отклонено"));
    }
    public override void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.ConnectionApprovalCallback = null;
        }
        base.OnDestroy();
    }
}
