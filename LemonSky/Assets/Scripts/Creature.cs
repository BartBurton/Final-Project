using UnityEngine;
using Unity.Netcode;
using System;

public abstract class Creature : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> _health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public event EventHandler OnCreatureDead;
    public event Action<int> OnHealthChanged;

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int value)
    {
        TakeDamage(value);
    }

    public void TakeDamage(int value)
    {
        _health.Value -= value;
        OnHealthChanged?.Invoke(_health.Value);

        if (_health.Value <= 0)
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
        return _health.Value;
    }
}
