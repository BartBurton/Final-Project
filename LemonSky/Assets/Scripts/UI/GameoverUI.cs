using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameoverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinCollected;
    [SerializeField] Button toMainMenuButton;

    private void Awake()
    {
        toMainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    void Start(){
        Hide();
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        CoinsManager.Instance.OnCoinCollected += CoinsManager_OnStateChanged;
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            if (GameManager.Instance.IsGameOver())
                Show();
            else
                Hide();
        };
    }

    void GameManager_OnStateChanged(object sender, System.EventArgs e){
        if(GameManager.Instance.IsGameOver())
            Show();
        else
            Hide();
    }

    void CoinsManager_OnStateChanged(int count)
    {
        coinCollected.text = count.ToString();
    }

    void Show(){
        Cursor.visible = true;
        gameObject.SetActive(true);
    }
    void Hide(){
        Cursor.visible = false;
        gameObject.SetActive(false);
    }
}
