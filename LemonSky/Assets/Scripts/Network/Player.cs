using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class Player : Creature
{
    private PlayerHudManager _hud;
    public override void OnNetworkSpawn()
    {
        _hud = GetComponent<PlayerHudManager>();

        _hud.HeatlthBar.SetMaxHealth(Health.Value);

        Health.OnValueChanged = (int prevVal, int newVal) =>
        {
            _hud.HeatlthBar.SetHealth(Health.Value);

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
