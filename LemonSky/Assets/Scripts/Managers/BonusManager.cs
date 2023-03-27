using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class BonusManager : NetworkBehaviour
{
    public static BonusManager Instance { get; private set; }

    int collectedCoins = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    public int GetCollectedCoins()
    {
        return collectedCoins;
    }
}
