using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class DeathUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] Button exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
        LocalUIManager.Instance.OnStateChanged += LocalUIStateChanged;
        CoinsManager.Instance.OnCoinCollected += CoinsManager_OnStateChanged;
    }

    void CoinsManager_OnStateChanged(int count)
    {
        resultText.text = count.ToString();
    }

    void LocalUIStateChanged(LocalUIManager.UIState uIState)
    {
        if (uIState == LocalUIManager.UIState.Death)
            Show();
        else
            Hide();
    }

    void Show()
    {
        Cursor.visible = true;
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
