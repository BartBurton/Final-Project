using System;
using UnityEngine;
using Unity.Netcode;
using StarterAssets;
using System.Collections.Generic;
using System.Collections;

public class Player : Creature
{
    public static Player LocalInstance { get; private set; }

    [SerializeField] public NetworkVariable<float> NetPower = new(23.5f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> NetJumpHeight = new(3.2f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<float> UpJumpPercent = new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<float> UpProtectPercent = new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<float> UpPowerPercent = new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    NetworkVariable<bool> Immortal = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<NetworkString> Name = new((NetworkString)User.Name, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public bool IsImmortal => Immortal.Value;

    public float Power { get => NetPower.Value + NetPower.Value * UpPowerPercent.Value / 100; }
    public float JumpHeight { get => NetJumpHeight.Value + NetJumpHeight.Value * UpJumpPercent.Value / 100; }


    public static event EventHandler OnAnyPlayerSpawned;

    public override void OnNetworkSpawn()
    {
        Health.OnValueChanged += (float prev, float next) => { Debug.Log($"Клиент ({OwnerClientId}){Name.Value} изменил здоровье. Сейчас: {next} Ранее: {prev}"); };

        if (IsOwner)
        {
            LocalInstance = this;
            Name.Value = (NetworkString)User.Name;
        }
        if (IsServer)
        {
            OnCreatureDead += (object s, EventArgs e) =>
            {
                var clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new[] { OwnerClientId },
                    }
                };
                DeadClientRpc(clientRpcParams);
            };
        }
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }
    public void Heal(float value)
    {
        this.AddHealth(value);
    }
    public void Damage(float value)
    {
        if (IsImmortal) return;
        this.AddHealth(-value);
    }

    protected override void Dead()
    {
        Debug.Log($"Клиент {OwnerClientId}({Name.Value}) погиб");
        PlayStatisticManager.Instance.Dead(OwnerClientId);

        GetComponent<NetworkObject>().Despawn();
    }

    public IEnumerator SetImmortalTime(float time)
    {
        SetImmortal(true);
        Debug.Log($"{OwnerClientId} получил неуязвимость");
        yield return new WaitForSeconds(time);
        SetImmortal(false);
        Debug.Log($"{OwnerClientId} потерял неуязвимость");
    }
    public void SetImmortal(bool value)
    {
        Immortal.Value = value;
    }

    [ClientRpc]
    void DeadClientRpc(ClientRpcParams clientRpcParams = default)
    {
        LocalUIManager.Instance.CurrentUIState = LocalUIManager.UIState.Death;
    }
}
