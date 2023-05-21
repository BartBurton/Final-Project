using UnityEngine;
using StarterAssets;
using Unity.Netcode;
using UnityEngine.InputSystem.XR;

public class ToxicGoo : NetworkBehaviour
{
    [SerializeField] private float _contactDamage = 35;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");

        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player.GetComponent<NetworkObject>().OwnerClientId != NetworkManager.Singleton.LocalClientId) return;

            player.TakeDamage(_contactDamage);

            var newPosition = PlayerSpawner.Instance.NextPosition();
            var controller = player.GetComponent<CharacterController>();

            controller.enabled = false;
            controller.transform.position = newPosition;
            controller.enabled = true;
        }
    }
}
