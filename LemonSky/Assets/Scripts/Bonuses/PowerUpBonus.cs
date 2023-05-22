using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PowerUpBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        player.GetComponent<PlayerAffects>().ApplyPowerUp(Value, Duration, player.OwnerClientId);
    }
}
