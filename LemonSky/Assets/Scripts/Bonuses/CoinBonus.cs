using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        PlayStatisticManager.Instance.CoinCollected(player.OwnerClientId);
    }
}
