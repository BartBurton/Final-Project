using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class Player : Creature
{
    [SerializeField] TextMeshPro headText;
    public override void OnNetworkSpawn()
    {
        headText.text = Health.Value.ToString();
        Health.OnValueChanged = (int prevVal, int newVal) =>
        {
            headText.text = Health.Value.ToString();
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
