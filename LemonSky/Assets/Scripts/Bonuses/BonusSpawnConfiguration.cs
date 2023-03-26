using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BonusSpawnConfiguration
{
    public GameObject Prefab;
    [Range(0, 60)] public int RespawnDelay;
    [Range(0, 50)] public int StartCount;
    [Range(1, 100)] public int MaxCount;
    [HideInInspector] public int CurrentRespawnDelay = 0;
    [HideInInspector] public int CurrentCount = 0;
}
