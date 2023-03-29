using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BonusSpawnPoint : MonoBehaviour
{
    public bool IsOnceSpawn = false;

    [HideInInspector] public bool IsFree = true;

    public Vector3 Position { get => transform.position; }
}
