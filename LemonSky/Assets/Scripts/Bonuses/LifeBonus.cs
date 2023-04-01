using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        Debug.Log("Собрал жизнь!");
        Heal(player);
    }


    void Heal(Creature creature)
    {
        creature.TakeDamage(-this.Value);
    }
}
