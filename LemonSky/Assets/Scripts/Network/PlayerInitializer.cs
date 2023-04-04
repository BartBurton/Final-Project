using StarterAssets;
using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.Netcode;
using UnityEngine;


public enum PlayerSkinType
{
    Frog = 0,
    Duck = 1,
}


[Serializable]
public class PlayerConfiguration
{
    public PlayerSkinType Type;
    public GameObject ModelPrefab;
}


public class PlayerInitializer : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private List<PlayerConfiguration> _playerConfigurations;

    public static PlayerInitializer Instance;

    private Dictionary<PlayerSkinType, GameObject> _configsMap;

    private void Awake()
    {
        Instance = this;
        _configsMap = new Dictionary<PlayerSkinType, GameObject>();
        foreach (var config in _playerConfigurations)
        {
            _configsMap.Add(config.Type, config.ModelPrefab);
        }
    }

    public GameObject GetSkin(PlayerSkinType type)
    {
        return _configsMap[type];
    }

    public override void OnNetworkSpawn()
    {
        SpawnPlayerServerRpc(NetworkManager.LocalClientId);
        // GameManager.Instance.OnStateChanged += (obj, e) =>
        // {
        //     if (!GameManager.Instance.IsWaitingToStart())
        //     {
        //         SpawnPlayerServerRpc(NetworkManager.LocalClientId);
        //     }
        // };
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientId)
    {
        var config = _playerConfigurations[new System.Random().Next(_playerConfigurations.Count)];
        GameMultiplayer.Instance.SpawnPlayer(clientId, config, _playerPrefab);
    }
}
