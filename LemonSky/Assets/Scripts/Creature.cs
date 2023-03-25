using UnityEngine;
using Unity.Netcode;

public abstract class Creature : NetworkBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] protected NetworkVariable<int> Health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    /// Получение урона
    /// </summary>
    /// <param name="value"></param>
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

}
