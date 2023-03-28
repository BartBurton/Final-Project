using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBonus : BonusObject
{
    // Start is called before the first frame update
    void Start()
    {
        onPickUp += Consol;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
            PickUp();
    }
}
