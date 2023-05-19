using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        player.PlayerAffects.ApplyPowerUp(Value, Duration);
    }
}
