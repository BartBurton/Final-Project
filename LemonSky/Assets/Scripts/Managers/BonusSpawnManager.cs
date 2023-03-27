using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;


//TODO: Недоделаное говно, доделать это говно(: - https://youtu.be/7glCsF9fv3s?t=3626
public class BonusSpawnManager : NetworkBehaviour
{
    public static BonusSpawnManager Instance { get; private set; }


    [Header("Coins")]
    public bool UseCoins;
    [SerializeField][ConditionalHide("UseCoins", true)] BonusSpawnConfiguration coinConfig;

    [Header("Lives")]
    public bool UseLives;
    [SerializeField][ConditionalHide("UseLives", true)] BonusSpawnConfiguration lifeConfig;

    [Header("JumpUp")]
    public bool UseJumpUp;
    [SerializeField][ConditionalHide("UseJumpUp", true)] BonusSpawnConfiguration jumpUpConfig;

    [Header("ProtectUp")]
    public bool UseProtectUp;
    [SerializeField][ConditionalHide("UseProtectUp", true)] BonusSpawnConfiguration protectUpConfig;

    [Header("PowerUp")]
    public bool UsePowerUp;
    [SerializeField][ConditionalHide("UsePowerUp", true)] BonusSpawnConfiguration powerUpConfig;


    List<GameObject> _onceSpawnPoints;
    List<GameObject> _freeSpawnPoints;
    List<GameObject> _fillSpawnPoints;

    List<BonusSpawnConfiguration> _acceptedConfigs;

    List<Action> _spawners;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitAcceptedConfigs();
        InitSpawnPoints();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            StartCoroutine(Cor(1));
        }
    }

    private void InitAcceptedConfigs()
    {
        _acceptedConfigs = new List<BonusSpawnConfiguration>();
        if (UseCoins)
        {
            _acceptedConfigs.Add(coinConfig);
        }
        if (UseLives)
        {
            _acceptedConfigs.Add(lifeConfig);
        }
        if (UseJumpUp)
        {
            _acceptedConfigs.Add(jumpUpConfig);
        }
        if (UseProtectUp)
        {
            _acceptedConfigs.Add(protectUpConfig);
        }
        if (UsePowerUp)
        {
            _acceptedConfigs.Add(powerUpConfig);
        }
    }
    private void InitSpawnPoints()
    {
        var count = transform.childCount;
        var spawnPoints = new List<GameObject>(count);
        while (count-- > 0)
        {
            spawnPoints.Add(transform.GetChild(count).gameObject);
        }

        _onceSpawnPoints = new List<GameObject>(spawnPoints.Where(sp => sp.GetComponent<BonusSpawnPoint>().IsOnceSpawn));
        _freeSpawnPoints = new List<GameObject>(spawnPoints.Where(sp => !sp.GetComponent<BonusSpawnPoint>().IsOnceSpawn));
        _fillSpawnPoints = new List<GameObject>(_freeSpawnPoints.Count);
    }



    private void StartSpawn()
    {
        var startCounts = new List<int>();

        if (UseCoins)
        {
            _acceptedConfigs.Add(coinConfig);
            startCounts.Add(coinConfig.StartCount);
        }
        if (UseLives)
        {
            _acceptedConfigs.Add(lifeConfig);
            startCounts.Add(lifeConfig.StartCount);
        }
        if (UseJumpUp)
        {
            _acceptedConfigs.Add(coinConfig);
            startCounts.Add(coinConfig.StartCount);
        }
        if (UseProtectUp)
        {
            _acceptedConfigs.Add(coinConfig);
            startCounts.Add(coinConfig.StartCount);
        }
        if (UsePowerUp)
        {
            _acceptedConfigs.Add(coinConfig);
            startCounts.Add(coinConfig.StartCount);
        }

        for (int i = 0; i < _acceptedConfigs.Count; i++)
        {
            if (startCounts[i] == 0) continue;
            startCounts[i]--;

            if (_onceSpawnPoints.Count > 0)
            {

            }
            else if (_freeSpawnPoints.Count > 0)
            {

            }
        }
    }

    private void Spawn(BonusSpawnConfiguration config)
    {
        config.CurrentRespawnDelay++;
        if (config.CurrentRespawnDelay > config.RespawnDelay)
        {
            if (config.CurrentCount < config.MaxCount && _freeSpawnPoints.Count > 0)
            {
                Spawn(config);
            }
            config.CurrentRespawnDelay = 0;
        }
    }


    //[ClientRpc]
/*    private void Spawn(BonusSpawnConfiguration config)
    {
        var spawnPoint = _freeSpawnPoints[new System.Random().Next(_freeSpawnPoints.Count)];

        _freeSpawnPoints.Remove(spawnPoint);
        _fillSpawnPoints.Add(spawnPoint);
        config.CurrentCount++;

        var go = Instantiate(config.Prefab, spawnPoint.transform.position, Quaternion.identity);
        go.GetComponent<BonusObject>().onPickUp += (object sender, EventArgs e) =>
        {
            _fillSpawnPoints.Remove(spawnPoint);
            _freeSpawnPoints.Add(spawnPoint);
            config.CurrentCount--;
        };
        go.GetComponent<NetworkObject>().Spawn(true);
    }*/


    IEnumerator Cor(int sec)
    {
        yield return new WaitForSeconds(sec);

        _spawners = new List<Action>();
        foreach (var config in _acceptedConfigs)
        {
            _spawners.Add(() => { Spawn(config); });
        }

        StartSpawn();

        while (true)
        {
            yield return new WaitForSeconds(sec);
            //OnSpawn?.Invoke();
        }
    }
}
