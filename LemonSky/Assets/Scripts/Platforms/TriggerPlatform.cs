using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class TriggerPlatform : AutomaticPlatform
{
    protected bool IsAction = false;

    protected override void HandleAction()
    {
        IsAction = false;
        base.HandleAction();
    }

    public override void Action()
    {
        if (IsAction)
        {
            base.Action();
        }
    }

    public override void ActionStateUpdate(float actionStateDeltaTime)
    {
        if (IsAction)
        {
            base.ActionStateUpdate(actionStateDeltaTime);
        }
    }

    protected virtual void OnPlayerStay(Collider player) { return; }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAction = true;
            OnPlayerStay(other);
        }
    }
}
