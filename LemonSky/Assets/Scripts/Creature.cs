using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections;

public abstract class Creature : NetworkBehaviour
{
    [SerializeField] public NetworkVariable<float> Health = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] public NetworkVariable<bool> Immortal = new(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public bool IsImmortal => Immortal.Value;

    public event EventHandler OnCreatureDead;

    public virtual void TakeDamage(float value)
    {
        if (Immortal.Value) return;
        SetHealth(Health.Value - value);
    }

    public virtual void TakeHeal(float value)
    {
        SetHealth(Health.Value + value);
    }

    protected void SetHealth(float value)
    {
        if (Immortal.Value && value < Health.Value) return;
        if (value > 100) value = 100;
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
    public void SetImmortal(bool value)
    {
        Immortal.Value = value;
    }

    public IEnumerator SetImmortalTime(float time)
    {
        SetImmortal(true);
        yield return new WaitForSeconds(time);
        SetImmortal(false);
    }
}
