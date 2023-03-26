using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] List<SpawnZone> spawnZones;
    public static SpawnManager Instance;

    void Awake(){
        Instance = this;
    }
    public Vector3 NextPosition(){
        return spawnZones[new System.Random().Next(spawnZones.Count)].NextRandomPosition();
    }
}
