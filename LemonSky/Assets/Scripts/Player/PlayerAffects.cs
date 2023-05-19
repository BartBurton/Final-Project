using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;


public class PlayerAffects
{
    private class Affect
    {
        public bool IsDone;
        public float BaseValue;
        public float Duration;
        public Action DoneAction;
    }

    private Dictionary<string, Affect> _affects;

    private Player _player;

    public PlayerAffects(Player player)
    {
        _player = player;

        _affects = new() {
            { "jumpUp", new() {
                IsDone = true,
                BaseValue = player.JumpHeight,
                Duration = 0,
                DoneAction = () => { 
                    player.JumpHeight = _affects["jumpUp"].BaseValue;
                    _affects["jumpUp"].IsDone = true;
                 },
            } },
            { "protectUp", new() {
                IsDone = true,
                BaseValue = player.Protect,
                Duration = 0,
                DoneAction = () => { 
                    player.Protect = _affects["protectUp"].BaseValue;
                    _affects["protectUp"].IsDone = true;
                 },
            } },
            { "powerUp", new() {
                IsDone = true,
                BaseValue = player.Power,
                Duration = 0,
                DoneAction = () => { 
                    player.Power = _affects["powerUp"].BaseValue;
                    _affects["powerUp"].IsDone = true;
                },
            } }
        };
    }


    public void StateUpdate(float deltaTime)
    {
        foreach (var affect in _affects)
        {
            if (!affect.Value.IsDone)
            {
                affect.Value.Duration -= deltaTime;
                if(affect.Value.Duration <= 0)
                {
                    affect.Value.DoneAction();
                }
            }
        }
    }

    public void ApplyJumpUp(float value, float duration)
    {
        _player.JumpHeight = value;
        ApplyAffect("jumpUp", duration);
    }

    public void ApplyProtectUp(float value, float duration)
    {
        _player.Protect = value;
        ApplyAffect("protectUp", duration);
    }

    public void ApplyPowerUp(float value, float duration)
    {
        _player.Power = value;
        ApplyAffect("powerUp", duration);
    }

    private void ApplyAffect(string key, float duration)
    {
        var affect = _affects[key];
        affect.IsDone = false;
        affect.Duration = duration;
    }
}
