using UnityEngine;
using Unity.Netcode;
using System;

public abstract class Creature : NetworkBehaviour
{
    public event EventHandler OnCreatureDead;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] public NetworkVariable<int> Health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int value)
    {
        TakeDamage(value);
    }

    public void TakeDamage(int value)
    {
        //if(!IsOwner) return;
        Health.Value -= value;
        if (Health.Value > _maxHealth)
            Health.Value = _maxHealth;
        else
            if (Health.Value <= 0)
        {
            OnCreatureDead?.Invoke(this, EventArgs.Empty);
            Dead();
        }
    }
    protected virtual void Dead()
    {
        Destroy(this.gameObject);
    }

    public int GetHealth()
    {
        return Health.Value;
    }

}
