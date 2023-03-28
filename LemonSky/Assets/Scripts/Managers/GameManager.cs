using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalPlayerReadyChanged;
    public static GameManager Instance { get; private set; }
    enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver
    }
    NetworkVariable<State> state = new(State.WaitingToStart);
    [SerializeField]
    [Tooltip("Ожидание начала игры")]
    float waitingToStartTimer = 1f;
    [SerializeField]
    [Tooltip("Отсчет до старта игры")]
    float countdownStartTimer = 3f;
    [SerializeField]
    [Tooltip("Длительность игры")]
    float gamePlayingTimerMax = 10f;
    float gamePlayingTimer = 10f;
    bool isLocalPlayerReady;
    Dictionary<ulong, bool> playersReadyDictionary;
    void Awake()
    {
        Instance = this;
        playersReadyDictionary = new();
    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        base.OnNetworkSpawn();
    }
    void Start()
    {
        GameInputs.Instance.OnInteractAction += GameInputs_OnInteractAction;
    }
    void Update()
    {
        if (!IsServer) return;
        switch (state.Value)
        {
            case State.WaitingToStart:
                // waitingToStartTimer -= Time.deltaTime;
                // if (waitingToStartTimer <= 0f){
                //     state = State.CountDownToStart;
                //     OnStateChanged?.Invoke(this, EventArgs.Empty);
                // }
                break;
            case State.CountDownToStart:
                countdownStartTimer -= Time.deltaTime;
                if (countdownStartTimer <= 0f)
                {
                    state.Value = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f)
                    state.Value = State.GameOver;
                break;
            case State.GameOver:
                break;
        }
    }
    public float GetCountdownToStartTimer()
    {
        return countdownStartTimer;
    }
    public float GetGameplayingTimerNormalize()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }
    public bool IsCountDownToStartActive()
    {
        return state.Value == State.CountDownToStart;
    }
    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }
    public bool IsWaitingToStart()
    {
        return state.Value == State.WaitingToStart;
    }
    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }
    void GameInputs_OnInteractAction(object sender, EventArgs e)
    {
        if (state.Value == State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            SetPlayerReadyServerRpc();
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    void State_OnValueChanged(State prevVal, State newVal)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
    [ServerRpc(RequireOwnership = false)]
    void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playersReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playersReadyDictionary.ContainsKey(clientId) || !playersReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }
        if(allClientsReady)
            state.Value = State.CountDownToStart;
        Debug.Log("All Ready - " + allClientsReady);
    }
}
