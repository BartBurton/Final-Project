using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputs : MonoBehaviour
{
    public static GameInputs Instance;
    PlayerInputs _playerInputs;
    public event EventHandler OnInteractAction;

    void Awake()
    {
        Instance = this;
        _playerInputs = new PlayerInputs();
    }

    void OnEnable()
    {
        _playerInputs.Player.Enable();
        _playerInputs.Player.Interact.performed += Interact_performed;
        _playerInputs.Player.Pause.performed += Pause_performed;
    }
    void OnDisable()
    {
        _playerInputs.Player.Disable();
        _playerInputs.Player.Interact.performed -= Interact_performed;
        _playerInputs.Player.Pause.performed -= Pause_performed;
    }

    public Vector2 MoveVector()
    {
        return _playerInputs.Player.Move.ReadValue<Vector2>().normalized;
    }
    public bool IsSprint()
    {
        return _playerInputs.Player.Sprint.ReadValue<float>() > 0;
    }

    public bool IsJump()
    {
        return _playerInputs.Player.Jump.ReadValue<float>() > 0;
    }
    public Vector2 LookVector()
    {
        return _playerInputs.Player.Look.ReadValue<Vector2>();
    }
    void Interact_performed(InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    void Pause_performed(InputAction.CallbackContext context)
    {
        if (LocalUIManager.Instance.CurrentUIState == LocalUIManager.UIState.Paused)
        {
            LocalUIManager.Instance.CurrentUIState = LocalUIManager.UIState.GamePlay;
        }
        else
        {
            LocalUIManager.Instance.CurrentUIState = LocalUIManager.UIState.Paused;
        }
    }
}
