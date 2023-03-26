using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawnPoint : MonoBehaviour
{
    [HideInInspector]
    public bool IsFree = true;
    [HideInInspector]
    public Vector3 Position = Vector3.zero;

    void Start()
    {
        Position = transform.position;
    }
}
