using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProtectUpBonus : BonusObject
{
    protected override void PickUp(Player player)
    {
        player.GetComponent<PlayerAffects>().ApplyProtectUpClientRpc(
            Value,
            Duration,
            new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { player.OwnerClientId } } }
        );
    }
}
