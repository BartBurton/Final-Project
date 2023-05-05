using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlatformsPool : NetworkBehaviour
{
    [SerializeField][Min(.1f)] private float _platformsActionStateUpdateDelay = .1f;

    private readonly List<IPlatform> _platforms = new();

    public override void OnNetworkSpawn()
    {
        var count = transform.childCount;
        while (count-- > 0)
        {
            if(transform.GetChild(count).gameObject.TryGetComponent(out IPlatform component))
            {
                _platforms.Add(component);
            }
        }

        StartCoroutine(PlatformsActionStateUpdate(_platformsActionStateUpdateDelay));

        base.OnNetworkSpawn();
    }

    private void Update()
    {
        foreach (var platform in _platforms)
        {
            platform.Action();
        }
    }

    private IEnumerator PlatformsActionStateUpdate(float updateDelay) {
        while (true)
        {
            yield return new WaitForSeconds(updateDelay);
            foreach (var platform in _platforms)
            {
                platform.ActionStateUpdate(updateDelay);
            }
        }
    }
}
