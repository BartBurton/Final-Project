using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MovementPlatformMovePlayer : NetworkBehaviour
{
    [SerializeField] private NetworkObject _connector;

    private readonly List<CharacterController> _playerControllers = new();

    public void OffPlayersMovement()
    {
        if (!IsServer) return;

        foreach (var item in _playerControllers)
        {
            item.enabled = false;
        }
    }

    public void OnPlayersMovement()
    {
        if (!IsServer) return;

        foreach (var item in _playerControllers)
        {
            item.enabled = true;
        }
    }

    public void Clear()
    {
        if (!IsServer) return;

        foreach (var item in _playerControllers)
        {
            item.enabled = true;
            item.transform.parent = null;
        }
        _playerControllers.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            if (!_playerControllers.Contains(other.GetComponent<CharacterController>()))
            {
                other.transform.parent = _connector.transform;
                _playerControllers.Add(other.GetComponent<CharacterController>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
            other.GetComponent<CharacterController>().enabled = true;
            _playerControllers.Remove(other.GetComponent<CharacterController>());
        }
    }
}
