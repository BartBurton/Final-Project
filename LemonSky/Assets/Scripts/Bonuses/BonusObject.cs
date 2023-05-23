using System;
using UnityEngine;
using Unity.Netcode;

public enum BonusType
{
    Coin,
    Life,
    UpJump,
    UpProtect,
    UpPower
}

public abstract class BonusObject : NetworkBehaviour
{
    [SerializeField] protected int Value;
    [SerializeField] protected int Duration;

    public BonusType Type;
    public event EventHandler OnPickUp;

    bool _isPickedUp = false;

    void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);

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
