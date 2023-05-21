using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using StarterAssets;

public class IronFront : NetworkBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !IsLocalPlayer) return;

        var player = GetComponent<Player>();
        player.PunchServerRpc(this.gameObject.transform.forward, player.OwnerClientId);
    }
}
