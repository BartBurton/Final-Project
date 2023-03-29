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


    List<BonusSpawnPoint> _onceSpawnPoints;
    List<BonusSpawnPoint> _spawnPoints;

    List<BonusSpawnConfiguration> _availableConfigs;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitAvailableConfigs();
        InitSpawnPoints();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            StartCoroutine(SpawnCor(1));
        }
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
        var spawnPoints = new List<BonusSpawnPoint>(count);
        while (count-- > 0)
        {
            var spawnPoint = transform.GetChild(count).gameObject.GetComponent<BonusSpawnPoint>();
            spawnPoint.IsFree = true;
            spawnPoints.Add(spawnPoint);
        }

        _onceSpawnPoints = new List<BonusSpawnPoint>(spawnPoints.Where(sp => sp.IsOnceSpawn));
        _spawnPoints = new List<BonusSpawnPoint>(spawnPoints.Where(sp => !sp.IsOnceSpawn));
    }


    private int FreeSpawnPointsCount
    {
        get => _spawnPoints.Where(sp => sp.IsFree).Count();
    }
    private int FreeOnceSpawnPointsCount
    {
        get => _onceSpawnPoints.Where(sp => sp.IsFree).Count();
    }
    private int GetRandomFreeSpawnPointIndex(List<BonusSpawnPoint> spawnPoints)
    {
        var freeSpawnPointsIndexes = spawnPoints.Select((sp, i) => sp.IsFree ? i : -1).Where(i => i != -1).ToList();
        return freeSpawnPointsIndexes[new System.Random().Next(freeSpawnPointsIndexes.Count)];
    }


    private void StartSpawn()
    {
        var startCounts = _availableConfigs.Select(ac => ac.StartCount).ToList();
        var maxStartCount = startCounts.Max();

        while (maxStartCount > 0)
        {
            if (FreeSpawnPointsCount > 0)
            {
                for (int i = 0; i < startCounts.Count; i++)
                {
                    if (startCounts[i] == 0) continue;
                    startCounts[i]--;

                    SpawnBonus(i, GetRandomFreeSpawnPointIndex(_spawnPoints), false);

                    if (FreeSpawnPointsCount == 0)
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

            if (FreeOnceSpawnPointsCount> 0)
            {
                for (int i = 0; i < startCounts.Count; i++)
                {

                    if (startCounts[i] == 0) continue;
                    startCounts[i]--;

                    SpawnBonus(i, GetRandomFreeSpawnPointIndex(_onceSpawnPoints), true);

                    if (FreeOnceSpawnPointsCount == 0)
                    {
                        break;
                    }
                }
                maxStartCount--;
            }

            if (FreeSpawnPointsCount == 0 && FreeOnceSpawnPointsCount == 0)
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
                SpawnBonus(configIndex, freePointIndex, false);
            }
            config.CurrentRespawnDelay = 0;
        }
    }


    private void SpawnBonus(int configIndex, int pointIndex, bool isOnce)
    {
        var config = _availableConfigs[configIndex];
        var point = !isOnce ? _spawnPoints[pointIndex] : _onceSpawnPoints[pointIndex];

        var go = Instantiate(config.Prefab, point.Position, Quaternion.identity);

        config.CurrentCount++;

        if (!isOnce)
        {
            _spawnPoints[pointIndex].IsFree = false;
        }
        else
        {
            _onceSpawnPoints[pointIndex].IsFree = false;
        }

        go.GetComponent<BonusObject>().onPickUp += (object sender, EventArgs e) =>
        {
            DespawnBonusServerRpc(configIndex, pointIndex, isOnce);
        };

        go.GetComponent<NetworkObject>().Spawn(true);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnBonusServerRpc(int configIndex, int pointIndex, bool isOnce)
    {
        DespawnBonusClientRpc(configIndex, pointIndex, isOnce);
    }

    [ClientRpc]
    private void DespawnBonusClientRpc(int configIndex, int pointIndex, bool isOnce)
    {
        var config = _availableConfigs[configIndex];
        config.CurrentCount--;

        if (!isOnce)
        {
            _spawnPoints[pointIndex].IsFree = true;
        }
    }


    IEnumerator SpawnCor(int sec)
    {
        yield return new WaitForSeconds(sec);
        StartSpawn();

        List<int> configIndexes = _availableConfigs.Select((e, i) => i).ToList();
        List<int> usedIndexes = new(configIndexes.Count);
        var rand = new System.Random();
        int pastSec = sec;
        while (true)
        {
            yield return new WaitForSeconds(sec);

            if (FreeSpawnPointsCount > 0)
            {
                if (configIndexes.Count == usedIndexes.Count)
                {
                    usedIndexes.Clear();
                    pastSec = sec;
                }

                var unusedIndexes = configIndexes.Except(usedIndexes).ToList();
                var index = unusedIndexes[rand.Next(unusedIndexes.Count)];
                usedIndexes.Add(index);

                TrySpawn(index,GetRandomFreeSpawnPointIndex(_spawnPoints), pastSec);

                pastSec++;
            }
            else
            {
                pastSec = sec;
            }
        }
    }
}
