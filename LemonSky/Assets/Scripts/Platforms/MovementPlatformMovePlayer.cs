using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MovementPlatformMovePlayer : NetworkBehaviour
{
    [SerializeField] private NetworkObject _connector;

    private readonly List<CharacterController> _playerControllers = new();

    private CharacterController _playerController = null;

    public void OffPlayersMovement()
    {
        if(_playerController != null)
        {
            _playerController.enabled = false;
        }

        //foreach (var item in _playerControllers)
        //{
        //    item.enabled = false;
        //}
    }

    public void OnPlayersMovement(Vector3 move)
    {
        if (_playerController != null)
        {
            _playerController.transform.Translate(move, transform);
            _playerController.enabled = true;
        }

        //foreach (var item in _playerControllers)
        //{
        //    item.enabled = true;
        //}
    }

    public void Clear()
    {
        if (_playerController != null)
        {
            _playerController.enabled = true;
        }
        _playerController = null;

        //foreach (var item in _playerControllers)
        //{
        //    item.enabled = true;
        //    item.transform.parent = null;
        //}
        //_playerControllers.Clear();
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<NetworkObject>().OwnerClientId != NetworkManager.Singleton.LocalClientId) return;

            _playerController = other.GetComponent<CharacterController>();

            //if (!_playerControllers.Contains(other.GetComponent<CharacterController>()))
            //{
            //    other.transform.parent = _connector.transform;
            //    _playerControllers.Add(other.GetComponent<CharacterController>());
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<NetworkObject>().OwnerClientId != NetworkManager.Singleton.LocalClientId) return;

            Clear();

            //other.transform.parent = null;
            //other.GetComponent<CharacterController>().enabled = true;
            //_playerControllers.Remove(other.GetComponent<CharacterController>());
        }
    }
}
