using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectUpBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        player.PlayerAffects.ApplyProtectUp(Value, Duration);
    }
}
