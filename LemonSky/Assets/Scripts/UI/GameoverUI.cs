using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameoverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinCollected;
    [SerializeField] Button toMainMenuButton;

    private void Awake()
    {
        toMainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    void Start(){
        Hide();
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    void GameManager_OnStateChanged(object sender, System.EventArgs e){
        if(GameManager.Instance.IsGameOver())
            Show();
        else
            Hide();
    }
    void Update(){
        coinCollected.text = BonusManager.Instance.GetCollectedCoins().ToString();
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
