using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : Creature
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
}
