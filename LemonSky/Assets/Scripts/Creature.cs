using UnityEngine;
using Unity.Netcode;

public abstract class Creature : NetworkBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    protected NetworkVariable<int> Health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        Health.OnValueChanged = (int prevVal, int newVal) =>
        {
            Debug.Log("Клиент №" + OwnerClientId + " получил урон(" + (prevVal - newVal) + ". Текущее HP: " + Health.Value);
        };
    }

    /// Получение урона
    /// </summary>
    /// <param name="value"></param>
    public void TakeDamage(int value)
    {
        Health.Value -= value;
        if (Health.Value > _maxHealth)
            Health.Value = _maxHealth;
        else
            if (Health.Value <= 0)
            Dead();
    }

    private void Dead()
    {

    }

}
