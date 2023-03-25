using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class BonusObject : NetworkBehaviour
{
    protected event EventHandler onPickUp;
    [SerializeField] protected BonusType type;
    [SerializeField] protected int value;
    public void PickUp(){
        onPickUp?.Invoke(this, EventArgs.Empty);
    }
    
    public void Destroy(object sender, EventArgs e){
        Destroy(this.gameObject);
        Debug.Log("Уничтожение бонуса " + type.ToString());
    }
    protected void Consol(object sender, EventArgs e){
        Debug.Log("Взял бонус " + type.ToString());
    }
    public enum BonusType{
        Coin,
        Life,
        Jump,
        Protect,
        Weapon
    }
}
