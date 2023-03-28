using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions.Must;


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

    List<BonusSpawnConfiguration> _availableConfigs;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitAvailableConfigs();
        InitSpawnPoints();
        StartCoroutine(SpawnCor(1));
    }

    public override void OnNetworkSpawn()
    {
        /*        if (IsServer)
                {
                    StartCoroutine(SpawnCor(1));
                }*/
    }

    private void InitAvailableConfigs()
    {
        _availableConfigs = new List<BonusSpawnConfiguration>();
        if (UseCoins)
        {
            _availableConfigs.Add(coinConfig);
        }
        if (UseLives)
        {
            _availableConfigs.Add(lifeConfig);
        }
        if (UseJumpUp)
        {
            _availableConfigs.Add(jumpUpConfig);
        }
        if (UseProtectUp)
        {
            _availableConfigs.Add(protectUpConfig);
        }
        if (UsePowerUp)
        {
            _availableConfigs.Add(powerUpConfig);
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
        var startCounts = _availableConfigs.Select(ac => ac.StartCount).ToList();
        var maxStartCount = startCounts.Max();

        var rand = new System.Random();

        while (maxStartCount > 0)
        {
            if (_freeSpawnPoints.Count > 0)
            {
                for (int i = 0; i < startCounts.Count; i++)
                {
                    if (startCounts[i] == 0) continue;
                    startCounts[i]--;

                    SpawnBonusClientRpc1(i, rand.Next(_freeSpawnPoints.Count), false);

                    if (_freeSpawnPoints.Count == 0)
                    {
                        break;
                    }
                }
                maxStartCount--;
            }

            if (maxStartCount == 0)
            {
                break;
            }

            if (_onceSpawnPoints.Count > 0)
            {
                for (int i = 0; i < startCounts.Count; i++)
                {

                    if (startCounts[i] == 0) continue;
                    startCounts[i]--;

                    SpawnBonusClientRpc1(i, rand.Next(_onceSpawnPoints.Count), true);

                    if (_onceSpawnPoints.Count == 0)
                    {
                        break;
                    }
                }
                maxStartCount--;
            }

            if (_freeSpawnPoints.Count == 0 && _onceSpawnPoints.Count == 0)
            {
                break;
            }
        }
    }

    private void TrySpawn(int configIndex, int freePointIndex, int pastSec)
    {
        var config = _availableConfigs[configIndex];
        config.CurrentRespawnDelay += pastSec;

        if (config.CurrentRespawnDelay > config.MinRespawnDelay)
        {
            if (config.CurrentCount < config.MaxCount)
            {
                SpawnBonusClientRpc1(configIndex, freePointIndex, false);
            }
            config.CurrentRespawnDelay = 0;
        }
    }


    //[ClientRpc]
    private void SpawnBonusClientRpc1(int configIndex, int pointIndex, bool isOnce)
    {
        var config = _availableConfigs[configIndex];
        var point = !isOnce ? _freeSpawnPoints[pointIndex] : _onceSpawnPoints[pointIndex];

        var go = Instantiate(config.Prefab, point.transform.position, Quaternion.identity);

        config.CurrentCount++;
        int fillPointIndex = -1;

        if (!isOnce)
        {
            _freeSpawnPoints.Remove(point);
            _fillSpawnPoints.Add(point);
            fillPointIndex = _fillSpawnPoints.Count - 1;
        }
        else
        {
            _onceSpawnPoints.Remove(point);
        }

        go.GetComponent<BonusObject>().onPickUp += (object sender, EventArgs e) =>
        {
            DespawnBonusServerRpc1(configIndex, fillPointIndex, isOnce);
        };

        go.GetComponent<NetworkObject>().Spawn(true);
    }

    //[ServerRpc(RequireOwnership = false)]
    private void DespawnBonusServerRpc1(int configIndex, int pointIndex, bool isOnce)
    {
        DespawnBonusClientRpc1(configIndex, pointIndex, isOnce);
    }

    //[ClientRpc]
    private void DespawnBonusClientRpc1(int configIndex, int pointIndex, bool isOnce)
    {
        var config = _availableConfigs[configIndex];
        config.CurrentCount--;

        if (!isOnce)
        {
            var point = _fillSpawnPoints[pointIndex];
            _fillSpawnPoints.Remove(point);
            _freeSpawnPoints.Add(point);
        }
    }


    IEnumerator SpawnCor(int sec)
    {
        yield return new WaitForSeconds(10);
        StartSpawn();

        List<int> configIndexes = _availableConfigs.Select((e, i) => i).ToList();
        List<int> usedIndexes = new(configIndexes.Count);
        var rand = new System.Random();
        int pastSec = sec;
        while (true)
        {
            yield return new WaitForSeconds(sec);

            if (_freeSpawnPoints.Count > 0)
            {
                if (configIndexes.Count == usedIndexes.Count)
                {
                    usedIndexes.Clear();
                    pastSec = sec;
                }

                var unusedIndexes = configIndexes.Except(usedIndexes).ToList();
                var index = unusedIndexes[rand.Next(unusedIndexes.Count)];
                usedIndexes.Add(index);

                TrySpawn(index, rand.Next(_freeSpawnPoints.Count), pastSec);

                pastSec++;
            }
            else
            {
                pastSec = sec;
            }
        }
    }
}
