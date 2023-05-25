using System;
using UnityEngine;
using Unity.Netcode;

public enum BonusType
{
    Coin,
    Life,
    UpJump,
    UpProtect,
    UpPower
}

public abstract class BonusObject : NetworkBehaviour
{
    [SerializeField] protected int Value;
    [SerializeField] protected int Duration;

    public BonusType Type;
    public event EventHandler OnPickUp;

    bool _isPickedUp = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (IsServer && !_isPickedUp)
            {
                PickUp(other.GetComponent<Player>());
                OnPickUp?.Invoke(this, EventArgs.Empty);
                _isPickedUp = true;

                return;
            }

            var audioNet = GetComponent<AudioNet>();
            AudioShot.Instance.PlaySafely(audioNet.GetAudioSource(other.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.Singleton.LocalClientId));
            gameObject.SetActive(false);
        }
    }

    protected abstract void PickUp(Player player);
}
