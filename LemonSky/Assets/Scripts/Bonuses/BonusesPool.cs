using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;


public class BonusesPool : NetworkBehaviour
{
    public static BonusesPool Instance { get; private set; }

    [SerializeField]
    private List<BonusSpawnPoint> _spawnPoints;

    [SerializeField]
    private List<BonusSpawnConfiguration> _availableBonusConfigs;

    private List<BonusSpawnPoint> _multipleSpawnPoints;
    private List<BonusSpawnPoint> _onceSpawnPoints;


    void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        InitSpawnPoints();
        StartCoroutine(SpawnCor(1));
    }

    private void InitSpawnPoints()
    {
        _multipleSpawnPoints = new(_spawnPoints.Where(sp => !sp.IsOnceSpawn));
        _onceSpawnPoints = new(_spawnPoints.Where(sp => sp.IsOnceSpawn));
    }


    private int GetRandomFreeSpawnPointIndex(BonusSpawnConfiguration config, List<BonusSpawnPoint> spawnPoints)
    {
        var freeSpawnPointsIndexes = spawnPoints
            .Select((sp, i) => sp.Bonus == null && sp.AvailableBonusType.Contains(config.Bonus.Type) ? i : -1)
            .Where(i => i != -1)
            .ToList();

        if (freeSpawnPointsIndexes.Count > 0)
        {
            return freeSpawnPointsIndexes[new System.Random().Next(freeSpawnPointsIndexes.Count)];
        }
        return -1;
    }


    private void StartSpawn()
    {
        bool TrySpawn(int configIndex, List<BonusSpawnPoint> spawnPoints, bool isOnce)
        {
            var config = _availableBonusConfigs[configIndex];

            if (config.StartCount > 0 && spawnPoints.Where(sp => sp.Bonus == null).Count() > 0)
            {
                var pointIndex = GetRandomFreeSpawnPointIndex(config, spawnPoints);

                if (pointIndex != -1)
                {
                    SpawnBonus(configIndex, pointIndex, isOnce);
                    config.StartCount--;
                    return true;
                }
            }

            return false;
        }

        bool isNotFill = true;

        while (isNotFill)
        {
            isNotFill = false;

            for (int i = 0; i < _availableBonusConfigs.Count; i++)
            {
                isNotFill |= TrySpawn(i, _onceSpawnPoints, true);
                isNotFill |= TrySpawn(i, _multipleSpawnPoints, false);
            }
        }
    }

    private void СontinuousSpawn(int configIndex, List<BonusSpawnPoint> spawnPoints, int pastSec)
    {
        var config = _availableBonusConfigs[configIndex];
        config.CurrentRespawnDelay += pastSec;

        if (config.CurrentRespawnDelay > config.MinRespawnDelay)
        {
            if (config.CurrentCount < config.MaxCount)
            {
                var pointIndex = GetRandomFreeSpawnPointIndex(config, spawnPoints);

                if (pointIndex != -1)
                {
                    SpawnBonus(configIndex, pointIndex, false);
                }
            }
            config.CurrentRespawnDelay = 0;
        }
    }


    private void SpawnBonus(int configIndex, int pointIndex, bool isOnce)
    {
        var config = _availableBonusConfigs[configIndex];
        var targetPoint = isOnce ? _onceSpawnPoints[pointIndex] : _multipleSpawnPoints[pointIndex];

        List<BonusSpawnPoint> spawnPoints = new(1) { targetPoint };

        if (targetPoint.IsSet)
        {
            spawnPoints = targetPoint.SpawnPoints;
            targetPoint.Bonus = new GameObject();
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPoints[i].Bonus != null) continue;

            config.CurrentCount++;

            var go = Instantiate(config.Bonus.gameObject, spawnPoints[i].Position, Quaternion.identity);
            spawnPoints[i].Bonus = go;

            int subPointIndex = i;

            go.GetComponent<BonusObject>().OnPickUp += (object sender, EventArgs e) =>
            {
                DespawnBonus(configIndex, pointIndex, isOnce, subPointIndex);
            };

            go.GetComponent<NetworkObject>().Spawn(true);
        }
    }

    private void DespawnBonus(int configIndex, int pointIndex, bool isOnce, int subPointIndex)
    {
        var config = _availableBonusConfigs[configIndex];
        config.CurrentCount--;

        var targetPoint = isOnce ? _onceSpawnPoints[pointIndex] : _multipleSpawnPoints[pointIndex];

        var spawnPoint = targetPoint.IsSet ? targetPoint.SpawnPoints[subPointIndex] : targetPoint;
        spawnPoint.Bonus.GetComponent<NetworkObject>().Despawn();
        spawnPoint.Bonus = null;

        targetPoint.Bonus = null;
    }


    IEnumerator SpawnCor(int sec)
    {
        yield return new WaitForSeconds(sec);
        StartSpawn();

        if (_availableBonusConfigs.Count > 0)
        {
            List<int> configIndexes = _availableBonusConfigs.Select((e, i) => i).ToList();
            List<int> usedIndexes = new(configIndexes.Count);
            var rand = new System.Random();
            int pastSec = sec;

            while (true)
            {
                yield return new WaitForSeconds(sec);

                if (_multipleSpawnPoints.Where(sp => sp.Bonus == null).Count() > 0)
                {
                    if (configIndexes.Count == usedIndexes.Count)
                    {
                        usedIndexes.Clear();
                        pastSec = sec;
                    }

                    var unusedIndexes = configIndexes.Except(usedIndexes).ToList();
                    var index = unusedIndexes[rand.Next(unusedIndexes.Count)];
                    usedIndexes.Add(index);

                    СontinuousSpawn(index, _multipleSpawnPoints, pastSec);

                    pastSec++;
                }
                else
                {
                    pastSec = sec;
                }
            }
        }
    }
}
