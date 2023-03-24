using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameoverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinCollected;
    
    void Start(){
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
        gameObject.SetActive(true);
    }
    void Hide(){
        gameObject.SetActive(false);
    }
}
