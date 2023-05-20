using UnityEngine;
using Unity.Netcode;
using System;

public abstract class Creature : NetworkBehaviour
{
    public NetworkVariable<int> Health = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public event EventHandler OnCreatureDead;

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int value)
    {
        TakeDamage(value);
    }

    public void TakeDamage(int value)
    {
        Health.Value -= value;

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

    public virtual int GetHealth()
    {
        return Health.Value;
    }
}
