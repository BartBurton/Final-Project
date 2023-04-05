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
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionCallback;
        NetworkManager.Singleton.StartHost();
    }
    void NetworkManager_ConnectionCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelect.ToString() || NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT)
        {
            connectionApprovalResponse.Approved = false;
            return;
        }
        connectionApprovalResponse.Approved = true;
    }
    public void StartClient()
    {
        OnTryingJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += GameMultiplayer_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
    }

    void GameMultiplayer_OnClientDisconnectCallback(ulong clientId){
        OnFailJoinGame?.Invoke(this, EventArgs.Empty);
    }
    public void StartServer()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionCallback;

        NetworkManager.Singleton.StartServer();
    }
}
