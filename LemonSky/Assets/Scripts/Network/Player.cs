using System;
using UnityEngine;
using Unity.Netcode;
using StarterAssets;
using System.Collections.Generic;

public class Player : Creature
{
    public static Player LocalInstance { get; private set; }

    [SerializeField] public NetworkVariable<float> NetPower = new(23.5f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> NetJumpHeight = new(3.2f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<float> UpJumpPercent = new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<float> UpProtectPercent = new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<float> UpPowerPercent = new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<NetworkString> Name = new((NetworkString)User.Name, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public float Power { get => NetPower.Value + NetPower.Value * UpPowerPercent.Value / 100; }
    public float JumpHeight { get => NetJumpHeight.Value + NetJumpHeight.Value * UpJumpPercent.Value / 100; }


    public static event EventHandler OnAnyPlayerSpawned;

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

    public override void TakeDamage(float value)
    {
        SetHealth(Health.Value - (value - value * UpProtectPercent.Value / 100));
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
        PlayStatisticManager.Instance.Dead(OwnerClientId);

        GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    void DeadClientRpc(ClientRpcParams clientRpcParams = default)
    {
        LocalUIManager.Instance.CurrentUIState = LocalUIManager.UIState.Death;
    }
}
