using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameoverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _resultText;
    [SerializeField] Button _matchResultsButton;
    [SerializeField] Button _exitButton;

    private void Awake()
    {
        _exitButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    void Start(){
        Hide();
        LocalUIManager.Instance.OnStateChanged += LocalUI_OnStateChanged;
        PlayStatisticManager.Instance.OnCoinCollected += CoinsManager_OnStateChanged;
    }

    void CoinsManager_OnStateChanged(int count)
    {
        _resultText.text = count.ToString();
    }

    void Show(){
        gameObject.SetActive(true);
    }

    void Hide(){
        gameObject.SetActive(false);
    }

    void LocalUI_OnStateChanged(LocalUIManager.UIState uIState)
    {
        if (uIState == LocalUIManager.UIState.GameOver)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
