using UnityEngine;
using StarterAssets;
using Unity.Netcode;
using UnityEngine.InputSystem.XR;

public class ToxicGoo : NetworkBehaviour
{
    [SerializeField] private float _contactDamage = 33.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if(IsServer)
            {
                player.TakeDamage(_contactDamage);
                PlayStatisticManager.Instance.Fail(player.OwnerClientId);
            }

            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                var newPosition = PlayerSpawner.Instance.NextPosition();
                var controller = player.GetComponent<CharacterController>();

                controller.enabled = false;
                controller.transform.position = newPosition;
                controller.enabled = true;
            }
        }
    }
}
