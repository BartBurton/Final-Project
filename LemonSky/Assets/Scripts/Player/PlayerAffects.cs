using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using StarterAssets;

public class PlayerAffects : NetworkBehaviour
{
    private class Affect
    {
        public bool IsDone;
        public float BaseValue;
        public float Duration;
        public Action<float> SetHud;
        public Action DoneAction;
    }

    private Player _player;
    private ThirdPersonController _controller;
    private Dictionary<string, Affect> _affects;

    private void Start()
    {
        _controller = GetComponent<ThirdPersonController>();
        _player = GetComponent<Player>();

        _affects = new() {
            { "jumpUp", new() {
                IsDone = true,
                BaseValue = _controller.JumpHeight,
                Duration = 0,
                SetHud = (float duration) => {Hud.Instance.JumpUpBar.Set(duration); },
                DoneAction = () => {
                    _controller.JumpHeight = _affects["jumpUp"].BaseValue;
                    _affects["jumpUp"].IsDone = true;
                 },
            } },
            { "protectUp", new() {
                IsDone = true,
                BaseValue = _player.Protect,
                Duration = 0,
                SetHud = (float duration) => {Hud.Instance.ProtectUpBar.Set(duration); },
                DoneAction = () => {
                    _player.Protect = _affects["protectUp"].BaseValue;
                    _affects["protectUp"].IsDone = true;
                 },
            } },
            { "powerUp", new() {
                IsDone = true,
                BaseValue = _player.Power,
                Duration = 0,
                SetHud = (float duration) => {Hud.Instance.PowerUpBar.Set(duration); },
                DoneAction = () => {
                    _player.Power = _affects["powerUp"].BaseValue;
                    _affects["powerUp"].IsDone = true;
                },
            } }
        };
    }


    public void FixedUpdate()
    {
        if (!IsLocalPlayer) return;

        foreach (var affect in _affects)
        {
            if (!affect.Value.IsDone)
            {
                affect.Value.Duration -= Time.fixedDeltaTime;
                affect.Value.SetHud(affect.Value.Duration);

                if (affect.Value.Duration <= 0)
                {
                    affect.Value.SetHud(0);
                    affect.Value.DoneAction();
                }
            }
        }
    }

    [ClientRpc]
    public void ApplyJumpUpClientRpc(float value, float duration, ClientRpcParams clientRpcParams = default)
    {
        _controller.JumpHeight = value;

        Hud.Instance.JumpUpBar.SetMax(duration);
        Hud.Instance.JumpUpBar.Set(duration);

        ApplyAffect("jumpUp", duration);
    }

    [ClientRpc]
    public void ApplyProtectUpClientRpc(float value, float duration, ClientRpcParams clientRpcParams = default)
    {
        _player.Protect = value;

        Hud.Instance.ProtectUpBar.SetMax(duration);
        Hud.Instance.ProtectUpBar.Set(duration);

        ApplyAffect("protectUp", duration);
    }

    [ClientRpc]
    public void ApplyPowerUpClientRpc(float value, float duration, ClientRpcParams clientRpcParams = default)
    {
        float baseValue = _affects["powerUp"].BaseValue;
        _player.Power = baseValue + baseValue * (value / 100);

        Hud.Instance.PowerUpBar.SetMax(duration);
        Hud.Instance.PowerUpBar.Set(duration);

        ApplyAffect("powerUp", duration);
    }

    private void ApplyAffect(string key, float duration)
    {
        var affect = _affects[key];
        affect.IsDone = false;
        affect.Duration = duration;
    }
}
