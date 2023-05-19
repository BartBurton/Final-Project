using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BonusSpawnPoint : MonoBehaviour
{
    public List<BonusType> AvailableBonusType;

    [HideInInspector]
    public GameObject Bonus;

    public bool IsOnceSpawn = false;

    public bool IsSet = false;
    [ConditionalHide("IsSet", true)] 
    public List<BonusSpawnPoint> SpawnPoints;

    public Vector3 Position { get => transform.position; }
}
