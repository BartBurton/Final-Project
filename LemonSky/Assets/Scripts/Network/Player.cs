using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using TMPro;

public class Player : Creature
{
    public float Protect = 1.5f;
    public float Power = 5.5f;

    public static event EventHandler OnAnyPlayerSpawned;

    public static Player LocalInstance { get; private set; }

    public NetworkVariable<NetworkString> Name = new NetworkVariable<NetworkString>((NetworkString)User.Name, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
            Name.Value = (NetworkString)User.Name;
        }
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

        if (IsServer) NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
    }

    void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == OwnerClientId)
        {

        }
    }

    protected override void Dead()
    {
        Debug.Log($"Клиент {OwnerClientId}({Name.Value.ToString()}) погиб");
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { OwnerClientId },
            }
        };
        DeadClientRpc(clientRpcParams);
        GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    void DeadClientRpc(ClientRpcParams clientRpcParams = default)
    {
        LocalUIManager.Instance.CurrentUIState = LocalUIManager.UIState.Death;
    }
}
