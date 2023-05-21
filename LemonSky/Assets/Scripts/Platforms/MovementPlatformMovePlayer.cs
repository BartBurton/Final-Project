using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MovementPlatformMovePlayer : NetworkBehaviour
{
    [SerializeField] private NetworkObject _connector;

    private CharacterController _playerController = null;

    public void OffPlayersMovement()
    {
        if(_playerController != null)
        {
            _playerController.enabled = false;
        }
    }

    public void OnPlayersMovement(Vector3 move)
    {
        if (_playerController != null)
        {
            _playerController.transform.Translate(move, transform);
            _playerController.enabled = true;
        }
    }

    public void Clear()
    {
        if (_playerController != null)
        {
            _playerController.enabled = true;
        }
        _playerController = null;
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<NetworkObject>().OwnerClientId != NetworkManager.Singleton.LocalClientId) return;

            _playerController = other.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<NetworkObject>().OwnerClientId != NetworkManager.Singleton.LocalClientId) return;

            Clear();
        }
    }
}
