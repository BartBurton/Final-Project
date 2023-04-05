using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalUIManager : MonoBehaviour
{
    public enum UIState
    {
        Default = 0,
        Paused = 1,
        Death = 2,
    }

    public static LocalUIManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    public event Action<UIState> OnStateChanged;

    private UIState _uiState = UIState.Default;
    public UIState CurrentUIState
    {
        get => _uiState;
        set
        {
            if (CanChange(value))
            {
                _uiState = value;
                OnStateChanged?.Invoke(_uiState);
            }
        }
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) {
            CurrentUIState = UIState.Default;
        }
    }

    private bool CanChange(UIState state)
    {
        if (state == UIState.Default) return true;

        if (!GameManager.Instance.IsGamePlaying()) return false;

        if (state == UIState.Paused && CurrentUIState == UIState.Death) return false;

        return true;
    }
}
