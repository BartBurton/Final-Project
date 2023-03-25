using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : Creature
{
    public override void OnNetworkSpawn()
    {
        Health.OnValueChanged = (int prevVal, int newVal) =>
        {
            if (prevVal > newVal)
                Debug.Log("Клиент №" + OwnerClientId + " получил " + (prevVal - newVal) + " урона. Текущее HP: " + Health.Value);
                else
                Debug.Log("Клиент №" + OwnerClientId + " исцелил " + (newVal - prevVal) + " здоровья. Текущее HP: " + Health.Value);
        };
    }

    protected override void Dead()
    {
        Debug.Log("Клиент №" + OwnerClientId + " помер");
    }
}
