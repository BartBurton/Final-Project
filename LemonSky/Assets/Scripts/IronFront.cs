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
                playerTarget.TakeDamage(_playerOwner.Power);
            }

            if(playerTarget.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                playerTarget.GetComponent<ThirdPersonController>().Impulse(new Vector2(0, 1), _playerOwner.Power);
            }
        }
    }
}
