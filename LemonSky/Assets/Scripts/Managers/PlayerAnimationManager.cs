using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimationManager : NetworkBehaviour
{
    private bool _hasAnimator;
    private Animator _animator;
    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

    [HideInInspector] public float AnimationBlend = 0.0f;

    public void AssignAnimations()
    {
        _hasAnimator = TryGetComponent(out _animator);

        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    public void PlayMove()
    {
        if (_hasAnimator)
        {
            PlayMoveServerRpc(AnimationBlend);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void PlayMoveServerRpc(float animationBlend)
    {
        PlayMoveClientRpc(animationBlend);
    }
    [ClientRpc]
    private void PlayMoveClientRpc(float animationBlend)
    {
        _animator.SetFloat(_animIDSpeed, animationBlend);
        _animator.SetFloat(_animIDMotionSpeed, 1f);
    }

    public void PlayGrounded(bool grounded)
    {
        if (_hasAnimator)
        {
            PlayGroundedServerRpc(grounded);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void PlayGroundedServerRpc(bool grounded)
    {
        PlayGroundedClientRpc(grounded);
    }
    [ClientRpc]
    private void PlayGroundedClientRpc(bool grounded)
    {
        _animator.SetBool(_animIDGrounded, grounded);
    }

    public void PlayJump(bool jump)
    {
        if (_hasAnimator)
        {
            PlayJumpServerRpc(jump);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void PlayJumpServerRpc(bool jump)
    {
        PlayJumpClientRpc(jump);
    }
    [ClientRpc]
    private void PlayJumpClientRpc(bool jump)
    {
        _animator.SetBool(_animIDJump, jump);
    }

    public void PlayFreeFall(bool fall)
    {
        if(_hasAnimator)
        {
            PlayFreeFallServerRpc(fall);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void PlayFreeFallServerRpc(bool fall)
    {
        PlayFreeFallClientRpc(fall);
    }
    [ClientRpc]
    private void PlayFreeFallClientRpc(bool fall)
    {
        _animator.SetBool(_animIDFreeFall, fall);
    }

    public void VoidAnnimations(bool grounded)
    {
        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, 0f);
            _animator.SetFloat(_animIDMotionSpeed, 0f);
            _animator.SetBool(_animIDGrounded, grounded);
            _animator.SetBool(_animIDJump, false);
            _animator.SetBool(_animIDFreeFall, !grounded);
        }
    }
}
