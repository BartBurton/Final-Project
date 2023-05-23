using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;



[Serializable]
public class PlayerConfiguration
{
    public PlayerType Type;
    public GameObject ModelPrefab;
}


public class PlayerInitializer : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private List<PlayerConfiguration> _playerConfigurations;

    public static PlayerInitializer Instance { get; private set; }

    private Dictionary<PlayerType, GameObject> _configsMap;

    private void Awake()
    {
        Instance = this;

        _configsMap = new Dictionary<PlayerType, GameObject>();
        foreach (var config in _playerConfigurations)
        {
            _configsMap.Add(config.Type, config.ModelPrefab);
        }
    }

    public GameObject GetBasePlayerPrefab()
    {
        return _playerPrefab;
    }

    public GameObject GetSkin(PlayerType type)
    {
        if (_configsMap.ContainsKey(type))
        {
            return _configsMap[type];
        }
        return _configsMap[PlayerType.Duck];
    }

    public PlayerType GetSafePlayerType(PlayerType? type)
    {
        return type ?? PlayerType.Duck;
    }
}
