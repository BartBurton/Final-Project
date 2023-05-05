using Unity.Netcode;
using UnityEngine;

public abstract class AutomaticPlatform : NetworkBehaviour, IPlatform
{
    [SerializeField] protected float CalmDelay = 0;
    protected float CurrentCalmTime = 0;

    protected virtual void HandleAction() => CurrentCalmTime = 0;

    public virtual void Action()
    {
        if (CurrentCalmTime < CalmDelay) return;
        HandleAction();
    }

    public virtual void ActionStateUpdate(float actionStateDeltaTime) => CurrentCalmTime += actionStateDeltaTime;
}
