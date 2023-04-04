using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using StarterAssets;

public class GameMultiplayer : NetworkBehaviour
{
    public static GameMultiplayer Instance { get; private set; }
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
        connectionApprovalResponse.Approved = true;
    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
    public void SpawnPlayer(ulong clientId, PlayerConfiguration config, GameObject pref)
    {
        Debug.Log($"Spawn ID {clientId}");
        var gayObject = Instantiate(pref, SpawnManager.Instance.NextPosition(), Quaternion.identity);
        gayObject.GetComponent<ThirdPersonController>().SkinType.Value = (int)config.Type;

        gayObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }
}
