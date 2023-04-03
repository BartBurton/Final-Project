using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;


public class BonusSpawnManager : NetworkBehaviour
{
    private class BonusArea
    {
        public BonusSpawnPoint SpawnPoint;
        public GameObject Bonus;

        public BonusArea() { }
        public BonusArea(BonusSpawnPoint spawnPoint, GameObject bonuus)
        {
            SpawnPoint = spawnPoint;
            Bonus = bonuus;
        }
    }

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


    List<BonusArea> _onceBonusAreas;
    List<BonusArea> _bonusAreas;

    List<BonusSpawnConfiguration> _availableConfigs;

    void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        InitAvailableConfigs();
        InitSpawnPoints();
        StartCoroutine(SpawnCor(1));
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
            spawnPoints.Add(spawnPoint);
        }

        _onceBonusAreas = new(spawnPoints.Where(sp => sp.IsOnceSpawn).Select(sp => new BonusArea(sp, null)));
        _bonusAreas = new(spawnPoints.Where(sp => !sp.IsOnceSpawn).Select(sp => new BonusArea(sp, null)));
    }


    private int FreeSpawnPointsCount
    {
        get => _bonusAreas.Where(sp => sp.Bonus == null).Count();
    }
    private int FreeOnceSpawnPointsCount
    {
        get => _onceBonusAreas.Where(sp => sp.Bonus == null).Count();
    }
    private int GetRandomFreeSpawnPointIndex(List<BonusArea> bonusAreas)
    {
        var freeSpawnPointsIndexes = bonusAreas.Select((sp, i) => sp.Bonus == null ? i : -1).Where(i => i != -1).ToList();
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

                    SpawnBonus(i, GetRandomFreeSpawnPointIndex(_bonusAreas), false);

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

            if (FreeOnceSpawnPointsCount > 0)
            {
                for (int i = 0; i < startCounts.Count; i++)
                {

                    if (startCounts[i] == 0) continue;
                    startCounts[i]--;

                    SpawnBonus(i, GetRandomFreeSpawnPointIndex(_onceBonusAreas), true);

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
        var area = !isOnce ? _bonusAreas[pointIndex] : _onceBonusAreas[pointIndex];

        var go = Instantiate(config.Prefab, area.SpawnPoint.Position, Quaternion.identity);

        config.CurrentCount++;
        area.Bonus = go;

        go.GetComponent<BonusObject>().OnPickUp += (object sender, EventArgs e) =>
        {
            Debug.Log("Before Despawn - " + OwnerClientId);
            DespawnBonus(configIndex, pointIndex, isOnce);
        };

        go.GetComponent<NetworkObject>().Spawn(true);
    }

    private void DespawnBonus(int configIndex, int pointIndex, bool isOnce)
    {
        Debug.Log("Despawn - " + OwnerClientId);
        var config = _availableConfigs[configIndex];
        config.CurrentCount--;
        var bonusArea = isOnce ? _onceBonusAreas[pointIndex] : _bonusAreas[pointIndex];
        bonusArea.Bonus.GetComponent<NetworkObject>().Despawn();
        bonusArea.Bonus = null;
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

                TrySpawn(index, GetRandomFreeSpawnPointIndex(_bonusAreas), pastSec);

                pastSec++;
            }
            else
            {
                pastSec = sec;
            }
        }
    }
}
