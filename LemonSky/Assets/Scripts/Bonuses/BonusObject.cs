using System;
using UnityEngine;
using Unity.Netcode;

public abstract class BonusObject : NetworkBehaviour
{
    bool _isPickedUp = false;
    public enum BonusType
    {
        Coin,
        Life,
        UpJump,
        UpProtect,
        UpPower
    }

    [SerializeField] protected BonusType Type;
    [SerializeField] protected int Value;
    [SerializeField] protected int Duration;
    public event EventHandler OnPickUp;


    void OnTriggerEnter(Collider other)
    {
        if(!IsServer) return;
        if(_isPickedUp) return;
        if (other.gameObject.CompareTag("Player"))
        {
            PickUp(other.GetComponent<Player>());
            OnPickUp?.Invoke(this, EventArgs.Empty);
            _isPickedUp = true;
        }
    }

    protected abstract void PickUp(Player player);
}
