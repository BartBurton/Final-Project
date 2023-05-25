using UnityEngine;
using StarterAssets;
using Unity.Netcode;
using UnityEngine.InputSystem.XR;
using System.Numerics;

public class ToxicGoo : NetworkBehaviour
{
    [SerializeField] private float _contactDamage = 33.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            PlayStatisticManager.Instance.Fail(player.OwnerClientId);
            var clientRpcParams = new ClientRpcParams()
            {
                Send = new()
                {
                    TargetClientIds = new[] { player.OwnerClientId }
                }
            };
            TeleportClientRpc();

            if (player.IsImmortal) return;
            player.TakeDamage(_contactDamage);
            StartCoroutine(player.SetImmortalTime(5));
        }
    }
    [ClientRpc]
    void TeleportClientRpc(ClientRpcParams clientRpcParams = default)
    {
        var newPosition = PlayerSpawner.Instance.NextPosition();
        var controller = Player.LocalInstance.GetComponent<CharacterController>();

        controller.enabled = false;
        controller.transform.position = newPosition;
        controller.enabled = true;
    }
}
