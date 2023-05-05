using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class EditorMode : NetworkBehaviour
{
#if UNITY_EDITOR    
    void Start()
    {
        GameInputs.Instance.OnInteractAction += GameInputs_OnInteractAction;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            var targetClientIds = NetworkManager.Singleton.ConnectedClients.Keys.ToArray();

            SpawnManager.Instance.SpawnPlayerClientRpc(new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = targetClientIds,
                }
            });
        }
    }

    void GameInputs_OnInteractAction(object sender, EventArgs e)
    {

        if (GameManager.Instance.IsWaitingToStart())
        {
            NetworkManager.Singleton.StartHost();
        }
    }
#endif
}
