using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterSelectReady : NetworkBehaviour
{
    public static CharacterSelectReady Instance {get; private set;}
    Dictionary<ulong, bool> playersReadyDictionary;

    void Awake(){
        Instance = this;
        playersReadyDictionary = new();
    }
    public void SetPlayerReady(){
        SetPlayerReadyServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {

        Debug.Log(serverRpcParams.Receive.SenderClientId);
        playersReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playersReadyDictionary.ContainsKey(clientId) || !playersReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }
        Debug.Log("Все готовы - " + allClientsReady);
        if (allClientsReady)
            Loader.LoadNetwork(Loader.Scene.Game);
    }
}
