using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatformsPool : NetworkBehaviour
{
    [SerializeField] private List<AutomaticPlatform> _automaticPlatforms;
    [SerializeField][Min(.1f)] private float _platformsActionStateUpdateDelay = .1f;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        StartCoroutine(PlatformsActionStateUpdateCor(_platformsActionStateUpdateDelay));
    }

    private void Update()
    {
        _automaticPlatforms.ForEach(p => p.Action());
    }

    private IEnumerator PlatformsActionStateUpdateCor(float updateDelay)
    {
        while (true)
        {
            yield return new WaitForSeconds(updateDelay);
            PlatformsActionStateUpdate(updateDelay);
            PlatformsActionStateUpdateClientRpc(updateDelay);
        }
    }

    [ClientRpc]
    private void PlatformsActionStateUpdateClientRpc(float updateDelay)
    {
        PlatformsActionStateUpdate(updateDelay);
    }

    private void PlatformsActionStateUpdate(float updateDelay)
    {
        _automaticPlatforms.ForEach(p => p.ActionStateUpdate(updateDelay));
    }
}
