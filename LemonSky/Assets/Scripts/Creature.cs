using UnityEngine;
using Unity.Netcode;
using System;

public abstract class Creature : NetworkBehaviour
{
    public float Protect = 0f;
    public float Power = 5.5f;
    public NetworkVariable<float> Health = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public event EventHandler OnCreatureDead;

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float value)
    {
        TakeDamage(value);
    }

    public void TakeDamage(float value)
    {
        if (value < 0)
        {
            Health.Value -= value;
        }
        else
        {
            Health.Value -= value - value * (Protect / 100);
        }

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

    public virtual float GetHealth()
    {
        return Health.Value;
    }
}
