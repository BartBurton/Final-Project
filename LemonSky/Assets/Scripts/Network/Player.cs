using System;
using UnityEngine;
using Unity.Netcode;
using StarterAssets;

public class Player : Creature
{
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

    [ServerRpc]
    public void PunchServerRpc(Vector3 punchDirection, ulong targetClientId)
    {
        ClientRpcParams clientRpcParams = new()
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { targetClientId }
            }
        };

        PunchClientRpc(punchDirection, Power, clientRpcParams);
    }

    [ClientRpc]
    public void PunchClientRpc(Vector3 punchDirection, float power, ClientRpcParams clientRpcParams = default)
    {
        TakeDamage(power);
        GetComponent<ThirdPersonController>().Impulse(punchDirection, power);
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


    void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == OwnerClientId)
        {

        }
    }
}
