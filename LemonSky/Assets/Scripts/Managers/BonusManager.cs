using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class BonusManager : NetworkBehaviour
{
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


    private List<GameObject> freeSpawnPoints;
    private List<GameObject> fillSpawnPoints;


    event Action OnSpawn;

    public static BonusManager Instance { get; private set; }
    int collectedCoins = 0;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        int count = transform.childCount;
        freeSpawnPoints = new List<GameObject>(count);
        fillSpawnPoints = new List<GameObject>(count);
        while(count-- > 0)
        {
            freeSpawnPoints.Add(transform.GetChild(count).gameObject);
        }

        if (UseCoins) OnSpawn += () => { TrySpawn(coinConfig); };
        if (UseLives) OnSpawn += () => { TrySpawn(lifeConfig); };
        if (UseJumpUp) OnSpawn += () => { TrySpawn(jumpUpConfig); };
        if (UseProtectUp) OnSpawn += () => { TrySpawn(protectUpConfig); };
        if (UsePowerUp) OnSpawn += () => { TrySpawn(powerUpConfig); };
        StartCoroutine(Cor(1));
    }

    public int GetCollectedCoins()
    {
        return collectedCoins;
    }

    private void InitSpawn(BonusSpawnConfiguration config)
    {
        for (int i = 0; i < config.StartCount; i++)
        {
            if (freeSpawnPoints.Count > 0)
            {
                Spawn(config);
            }
            else
            {
                return;
            }
        }
    }

    private void TrySpawn(BonusSpawnConfiguration config)
    {
        config.CurrentRespawnDelay++;
        if (config.CurrentRespawnDelay > config.RespawnDelay)
        {
            if (config.CurrentCount < config.MaxCount && freeSpawnPoints.Count > 0)
            {
                Spawn(config);
            }
            config.CurrentRespawnDelay = 0;
        }
    }

    //[ClientRpc]
    private void Spawn(BonusSpawnConfiguration config)
    {
        var spawnPoint = freeSpawnPoints[new System.Random().Next(freeSpawnPoints.Count)];

        freeSpawnPoints.Remove(spawnPoint);
        fillSpawnPoints.Add(spawnPoint);
        config.CurrentCount++;

        var go = Instantiate(config.Prefab, spawnPoint.transform.position, Quaternion.identity);
        go.GetComponent<BonusObject>().onPickUp += (object sender, EventArgs e) =>
        {
            fillSpawnPoints.Remove(spawnPoint);
            freeSpawnPoints.Add(spawnPoint);
            config.CurrentCount--;
        };
        go.GetComponent<NetworkObject>().Spawn(true);
    }

    private 

    IEnumerator Cor(int sec)
    {
        while (true)
        {
            if (IsServer)
            {
                yield return new WaitForSeconds(sec);
                if (UseCoins) InitSpawn(coinConfig);
                if (UseLives) InitSpawn(lifeConfig);
                if (UseJumpUp) InitSpawn(jumpUpConfig);
                if (UseProtectUp) InitSpawn(protectUpConfig);
                if (UsePowerUp) InitSpawn(powerUpConfig);

                while (true)
                {
                    OnSpawn?.Invoke();
                    yield return new WaitForSeconds(sec);
                }
            }
            yield return new WaitForSeconds(sec);
        }
    }
}
