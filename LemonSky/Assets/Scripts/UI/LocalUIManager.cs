using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LocalUIManager : MonoBehaviour
{
    public enum UIState
    {
        None,
        WaitingToStart,
        WaitingPlayers,
        CountDownToStart,
        GamePlay,
        Paused,
        Death,
        Lock,
    }

    public static LocalUIManager Instance { get; private set; }

    private readonly Dictionary<UIState, List<UIState>> _uiMap = new()
    {
        { UIState.None, new() { UIState.WaitingToStart, UIState.WaitingPlayers, UIState.CountDownToStart, UIState.GamePlay, UIState.Paused, UIState.Death, UIState.Lock } },
        { UIState.WaitingToStart, new() { UIState.WaitingPlayers, UIState.CountDownToStart, UIState.GamePlay, UIState.Lock } },
        { UIState.WaitingPlayers, new() { UIState.CountDownToStart, UIState.GamePlay, UIState.Lock } },
        { UIState.CountDownToStart, new() { UIState.GamePlay, UIState.Lock } },
        { UIState.GamePlay, new() { UIState.Paused, UIState.Death, UIState.Lock } },
        { UIState.Paused, new() { UIState.GamePlay, UIState.Death, UIState.Lock } },
        { UIState.Death, new() { UIState.Lock } },
        { UIState.Lock, new() { } },
    };

    private readonly List<UIState> _cursorLokcs = new() { UIState.WaitingToStart, UIState.WaitingPlayers, UIState.CountDownToStart, UIState.GamePlay };

    public event Action<UIState, UIState> OnStateChanged;

    private UIState _uiState = UIState.None;
    public UIState CurrentUIState
    {
        get => _uiState;
        set
        {
            if (!_uiMap[_uiState].Contains(value)) return;

            OnStateChanged?.Invoke(_uiState, value);

            _uiState = value;


            if (_cursorLokcs.Contains(_uiState))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnStateChanged;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        SetGameUi();
    }

    void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        SetGameUi();
    }


    void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        SetGameUi();
    }

    private void SetGameUi()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (GameManager.Instance.IsGameOver())
        {
            CurrentUIState = UIState.None;
        }
        else if (GameManager.Instance.IsGamePlaying())
        {
            CurrentUIState = UIState.GamePlay;
        }
        else if(GameManager.Instance.IsCountDownToStartActive())
        {
            CurrentUIState = UIState.CountDownToStart;
        }
        else if(GameManager.Instance.IsLocalPlayerReady() && GameManager.Instance.IsWaitingToStart())
        {
            CurrentUIState = UIState.WaitingPlayers;
        }
        else if(!GameManager.Instance.IsLocalPlayerReady() && GameManager.Instance.IsWaitingToStart())
        {
            CurrentUIState = UIState.WaitingToStart;
        }
    }
}
