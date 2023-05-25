using UnityEngine;
using Unity.Netcode;
using StarterAssets;

public class IronFront : NetworkBehaviour
{
    [SerializeField] private Player _playerOwner;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player playerTarget = other.GetComponent<Player>();

            if (IsServer)
            {
                playerTarget.Damage(_playerOwner.Power);
                PlayStatisticManager.Instance.Punch(_playerOwner.OwnerClientId);
                PlayStatisticManager.Instance.Fail(playerTarget.OwnerClientId);
            }

            if (playerTarget.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                playerTarget.GetComponent<ThirdPersonController>().Impulse(
                    new Vector2(transform.forward.x, transform.forward.z), 
                    _playerOwner.Power
                );
            }
        }
    }
}
