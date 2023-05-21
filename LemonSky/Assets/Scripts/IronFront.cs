using UnityEngine;
using Unity.Netcode;

public class IronFront : NetworkBehaviour
{
    [SerializeField] private Player _player;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !IsLocalPlayer) return;

        _player.PunchServerRpc(
            gameObject.transform.forward,
            _player.Power,
            other.GetComponent<Player>().OwnerClientId
        );
    }
}
