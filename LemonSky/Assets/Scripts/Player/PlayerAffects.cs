using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerAffects : NetworkBehaviour
{
    private class Affect
    {
        public bool IsDone;
        public bool IsDoneHud;
        public float Duration;
        public Action<float> SetHud;
        public Action DoneAction;
    }

    private Dictionary<string, Affect> _affects;

    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();

        _affects = new() {
            { "jumpUp", new() {
                IsDone = true,
                IsDoneHud = true,
                Duration = 0,
                SetHud = (float duration) => { if(IsLocalPlayer){ AffectsHud.Instance.JumpUpBar.Set(duration); } },
                DoneAction = () => { if(IsServer){ _player.UpJumpPercent.Value = 0; } }
            } },
            { "protectUp", new() {
                IsDone = true,
                IsDoneHud = true,
                Duration = 0,
                SetHud = (float duration) => {if(IsLocalPlayer){ AffectsHud.Instance.ProtectUpBar.Set(duration); } },
                DoneAction = () => {if(IsServer){  _player.UpProtectPercent.Value = 0; } }
            } },
            { "powerUp", new() {
                IsDone = true,
                IsDoneHud = true,
                Duration = 0,
                SetHud = (float duration) => {if(IsLocalPlayer){ AffectsHud.Instance.PowerUpBar.Set(duration); } },
                DoneAction = () => { if(IsServer){ _player.UpPowerPercent.Value = 0; } }
            } }
        };
    }


    public void FixedUpdate()
    {
        if (IsServer)
        {
            foreach (var affect in _affects)
            {
                if (!affect.Value.IsDone)
                {
                    affect.Value.Duration -= Time.fixedDeltaTime;

                    if (affect.Value.Duration <= 0)
                    {
                        affect.Value.DoneAction();
                        affect.Value.IsDone = true;
                    }
                }
            }
        }

        if (IsLocalPlayer)
        {
            foreach (var affect in _affects)
            {
                if (!affect.Value.IsDoneHud)
                {
                    affect.Value.Duration -= Time.fixedDeltaTime;
                    affect.Value.SetHud(affect.Value.Duration);

                    if (affect.Value.Duration <= 0)
                    {
                        affect.Value.SetHud(0);
                        affect.Value.IsDoneHud = true;
                    }
                }
            }
        }
    }

    public void ApplyJumpUp(float value, float duration, ulong targetClientId)
    {
        _player.UpJumpPercent.Value = value;

        ApplyAffect("jumpUp", duration);
        ApplyJumpUpClientRpc(
            duration,
            new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { targetClientId } } }
        );
    }
    public void ApplyProtectUp(float value, float duration, ulong targetClientId)
    {
        _player.UpProtectPercent.Value = value;

        ApplyAffect("protectUp", duration);
        ApplyProtectUpClientRpc(
            duration,
            new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { targetClientId } } }
        );
    }
    public void ApplyPowerUp(float value, float duration, ulong targetClientId)
    {
        _player.UpPowerPercent.Value = value;

        ApplyAffect("powerUp", duration);
        ApplyPowerUpClientRpc(
            duration,
            new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { targetClientId } } }
        );
    }

    [ClientRpc]
    public void ApplyJumpUpClientRpc(float duration, ClientRpcParams clientRpcParams = default)
    {
        AffectsHud.Instance.JumpUpBar.SetMax(duration);
        AffectsHud.Instance.JumpUpBar.Set(duration);

        ApplyAffect("jumpUp", duration);
    }

    [ClientRpc]
    public void ApplyProtectUpClientRpc(float duration, ClientRpcParams clientRpcParams = default)
    {
        AffectsHud.Instance.ProtectUpBar.SetMax(duration);
        AffectsHud.Instance.ProtectUpBar.Set(duration);

        ApplyAffect("protectUp", duration);
    }

    [ClientRpc]
    public void ApplyPowerUpClientRpc(float duration, ClientRpcParams clientRpcParams = default)
    {
        AffectsHud.Instance.PowerUpBar.SetMax(duration);
        AffectsHud.Instance.PowerUpBar.Set(duration);

        ApplyAffect("powerUp", duration);
    }

    private void ApplyAffect(string key, float duration)
    {
        var affect = _affects[key];
        affect.IsDone = false;
        affect.IsDoneHud = false;
        affect.Duration = duration;
    }
}
