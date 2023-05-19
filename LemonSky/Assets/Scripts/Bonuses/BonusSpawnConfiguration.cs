using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BonusSpawnConfiguration
{
    public BonusObject Bonus;
    public int MinRespawnDelay;
    public int StartCount;
    public int MaxCount;
    [HideInInspector] public int CurrentRespawnDelay = 0;
    [HideInInspector] public int CurrentCount = 0;
}
