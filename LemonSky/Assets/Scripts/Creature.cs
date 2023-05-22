using UnityEngine;
using Unity.Netcode;
using System;

public abstract class Creature : NetworkBehaviour
{
    [SerializeField] public NetworkVariable<float> Health = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public event EventHandler OnCreatureDead;

    public virtual void TakeDamage(float value)
    {
        SetHealth(Health.Value - value);
    }

    public virtual void TakeHeal(float value)
    {
        SetHealth(Health.Value + value);
    }

    protected void SetHealth(float value)
    {
        Health.Value = value;

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
