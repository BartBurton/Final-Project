using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUpBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        player.PlayerAffects.ApplyJumpUp(Value, Duration);
    }
}
