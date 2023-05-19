using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        CoinsManager.Instance.CoinCollected(player.OwnerClientId);
    }
}
