using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnStateChanged;
    public static GameManager Instance {get; private set;}
    enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver
    }
    State state;
    [SerializeField]
    [Tooltip("Ожидание начала игры")]
    float waitingToStartTimer = 1f;
    [SerializeField]
    [Tooltip("Отсчет до старта игры")]
    float countdownStartTimer = 3f;
    [SerializeField]
    [Tooltip("Длительность игры")]
    float gamePlayingTimer = 10f;
    void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }
    void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f){
                    state = State.CountDownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountDownToStart:
                countdownStartTimer -= Time.deltaTime;
                if (countdownStartTimer <= 0f){
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f){
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
        Debug.Log(state);
    }
    public float GetCountdownToStartTimer(){
        return countdownStartTimer;
    }
    public bool IsGamePlaying(){
        return state == State.GamePlaying;
    }
    public bool IsCountDownToStartActive(){
        return state == State.CountDownToStart;
    }
    public bool IsGameOver(){
        return state == State.GameOver;
    }
}
