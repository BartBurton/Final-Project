using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections;

public abstract class Creature : NetworkBehaviour
{
    static readonly float MaxHealth = 100;
    [SerializeField] public NetworkVariable<float> Health = new(MaxHealth, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public event EventHandler OnCreatureDead;
    protected void AddHealth(float value)
    {
        Health.Value += value;
        if (Health.Value > MaxHealth) Health.Value = MaxHealth;
        if (Health.Value < 0) DeadInternal();
    }
    protected virtual void Dead()
    {
        Destroy(this.gameObject);
    }
    void DeadInternal()
    {
        OnCreatureDead?.Invoke(this, EventArgs.Empty);
        Dead();
    }
    public virtual float GetHealth()
    {
        return Health.Value;
    }
    
}
