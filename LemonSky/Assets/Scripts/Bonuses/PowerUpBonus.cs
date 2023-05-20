using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PowerUpBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        player.GetComponent<PlayerAffects>().ApplyPowerUpClientRpc(
            Value,
            Duration,
            new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { player.OwnerClientId } } }
        );
    }
}
