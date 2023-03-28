using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBonus : BonusObject
{
    // Start is called before the first frame update
    void Start()
    {
        onPickUp += Consol;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            Heal(other.GetComponent<Player>());
            PickUp();
        }
    }

    void Heal(Creature creature){
        creature.TakeDamage(-this.value);
    }

}
