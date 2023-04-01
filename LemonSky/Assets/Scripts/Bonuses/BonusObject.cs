using System;
using UnityEngine;
using Unity.Netcode;

public abstract class BonusObject : NetworkBehaviour
{
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
        if (other.gameObject.CompareTag("Player"))
        {
            PickUp(other.GetComponent<Player>());
            OnPickUp?.Invoke(this, EventArgs.Empty);
        }
    }

    protected abstract void PickUp(Player player);
}
