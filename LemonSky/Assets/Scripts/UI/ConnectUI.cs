using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConnectUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Label;
    void Start(){
        GameMultiplayer.Instance.OnTryingJoinGame += GameMultiplayer_OnTryingJoinGame;
        GameMultiplayer.Instance.OnFailJoinGame += GameMultiplayer_OnFailJoinGame;
        Hide();
    }

    void GameMultiplayer_OnTryingJoinGame(object sender, EventArgs e){
        Label.text = "Подключение...";
        Show();
    }
    void GameMultiplayer_OnFailJoinGame(object sender, EventArgs e){
        Label.text = "Ошибка подключения...";
        Show();
    }
    void Show(){
        gameObject.SetActive(true);
    }
    void Hide(){
        gameObject.SetActive(false); 
    }
    void OnDestroy(){
        GameMultiplayer.Instance.OnTryingJoinGame -= GameMultiplayer_OnTryingJoinGame;
        GameMultiplayer.Instance.OnFailJoinGame -= GameMultiplayer_OnFailJoinGame;
    }
}
