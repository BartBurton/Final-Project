using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProtectUpBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        player.GetComponent<PlayerAffects>().ApplyProtectUp(Value, Duration, player.OwnerClientId);

    }
}
