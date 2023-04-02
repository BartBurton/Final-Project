using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using TMPro;

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
        Health.OnValueChanged = (int prevVal, int newVal) =>
        {
            if (prevVal > newVal)
                Debug.Log("Клиент №" + OwnerClientId + " получил " + (prevVal - newVal) + " урона. Текущее HP: " + Health.Value);
            else
                Debug.Log("Клиент №" + OwnerClientId + " исцелил " + (newVal - prevVal) + " здоровья. Текущее HP: " + Health.Value);
        };
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }

    protected override void Dead()
    {
        Debug.Log("Клиент №" + OwnerClientId + " помер");
    }
}
