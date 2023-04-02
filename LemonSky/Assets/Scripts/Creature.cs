using UnityEngine;
using Unity.Netcode;

public abstract class Creature : NetworkBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] public NetworkVariable<int> Health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public void TakeDamage(int value)
    {
        if(!IsOwner) return;
        Health.Value -= value;
        if (Health.Value > _maxHealth)
            Health.Value = _maxHealth;
        else
            if (Health.Value <= 0)
            Dead();
    }

    protected virtual void Dead()
    {
        Destroy(this.gameObject);
    }

    public int GetHealth(){
        return Health.Value;
    }

}
