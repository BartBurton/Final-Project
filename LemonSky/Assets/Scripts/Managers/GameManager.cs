using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver
    }

    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalPlayerReadyChanged;

    public static GameManager Instance { get; private set; }
    NetworkVariable<State> state = new(State.WaitingToStart);

    [SerializeField]
    [Tooltip("Ожидание начала игры")]
    float waitingToStartTimer = 10f;

    [SerializeField]
    [Tooltip("Отсчет до старта игры")]
    NetworkVariable<float> countdownStartTimer = new(3f);

    [Tooltip("Длительность игры")]
    public static float GamePlayingTimerMax = 1000f;

    NetworkVariable<float> gamePlayingTimer = new(0f);

    bool isLocalPlayerReady;

    Dictionary<ulong, bool> playersReadyDictionary;

    void Awake()
    {
        Instance = this;
        playersReadyDictionary = new();
    }
    public override void OnNetworkSpawn()
    {
        _isSpawn = false;
        state.OnValueChanged += State_OnValueChanged;
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            //OnStateChanged += DisimmortalAll;
        }

        base.OnNetworkSpawn();
    }
    public override void OnNetworkDespawn()
    {
        state.OnValueChanged -= State_OnValueChanged;
        if (IsServer)
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
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
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f)
                    state.Value = State.CountDownToStart;
                break;
            case State.CountDownToStart:
                countdownStartTimer.Value -= Time.deltaTime;
                if (countdownStartTimer.Value <= 0f)
                {
                    state.Value = State.GamePlaying;
                    gamePlayingTimer.Value = GamePlayingTimerMax;
                    AudioShot.Instance.Play("start-gameplay");
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value <= 0f)
                    state.Value = State.GameOver;
                break;
            case State.GameOver:
                if (IsServer)
                {
                    GameOverClientRpc();
                    state.Dispose();
                    Loader.Load(Loader.Scene.CharacterSelect, true, false);
                }
                break;
        }
    }
    bool _isSpawn = false;
    void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode mode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (_isSpawn) return;

        PlayerSpawner.Instance.SpawnPlayerClientRpc(new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = clientsCompleted.ToArray(),
            }
        });

        _isSpawn = true;

    }

    public float GetCountdownToStartTimer()
    {
        return countdownStartTimer.Value;
    }
    public float GetGameplayingTimer()
    {
        return gamePlayingTimer.Value;
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
            AudioShot.Instance.Play("start-interaction");

            isLocalPlayerReady = true;

            Debug.Log(isLocalPlayerReady);
            SetPlayerReadyServerRpc();

            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    void State_OnValueChanged(State prevVal, State newVal)
    {
        Debug.Log(prevVal.ToString());
        Debug.Log(newVal.ToString());
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
    [ServerRpc(RequireOwnership = false)]
    void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log(serverRpcParams.Receive.SenderClientId);
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
        if (allClientsReady && state.Value == State.WaitingToStart)
            state.Value = State.CountDownToStart;
        Debug.Log(allClientsReady);
    }
    [ClientRpc]
    void GameOverClientRpc()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.Load(Loader.Scene.MatchResults, false, true);
    }

    public double GetCurrentTimeFromStart()
    {
        return GamePlayingTimerMax - gamePlayingTimer.Value;
    }

    void DisimmortalAll(object sender, EventArgs e)
    {
        if (!IsGamePlaying()) return;
        var players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.GetComponent<Player>()).ToList();
        players.ForEach(e => { Debug.Log($"Деактивация неуязвимости {e.Name.Value}"); e.SetImmortal(false); });
    }
}
