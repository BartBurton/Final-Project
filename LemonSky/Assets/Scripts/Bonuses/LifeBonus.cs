using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        player.TakeDamage(-Value);
    }
}
