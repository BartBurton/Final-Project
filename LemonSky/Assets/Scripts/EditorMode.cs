using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class EditorMode : NetworkBehaviour
{
#if UNITY_EDITOR    
    private async void Awake()
    {
        try
        {
            User.SetUser(await APIRequests.WhoIAm());
        }
        catch { }
    }

    void Start()
    {
        GameInputs.Instance.OnInteractAction += (s, e) =>
        {
            if (GameManager.Instance.IsWaitingToStart())
            {
                NetworkManager.Singleton.StartHost();
            }
        };
    }

    public override void OnNetworkSpawn()
    {
        if (!IsHost) return;

        var targetClientIds = NetworkManager.Singleton.ConnectedClients.Keys.ToArray();

        PlayerSpawner.Instance.SpawnPlayerClientRpc(new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = targetClientIds,
            }
        });
    }
#endif
}
