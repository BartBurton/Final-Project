using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _exitButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    void Start()
    {
        Hide();
        LocalUIManager.Instance.OnStateChanged += LocalUI_OnStateChanged;
        PlayStatisticManager.Instance.OnCoinCollected += CoinsManager_OnStateChanged;
    }

    void CoinsManager_OnStateChanged(int count)
    {
        _resultText.text = count.ToString();
    }

    void LocalUI_OnStateChanged(LocalUIManager.UIState uIState)
    {
        if (uIState == LocalUIManager.UIState.Death)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
