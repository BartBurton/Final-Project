using System;
using UnityEngine;
using TMPro;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;

    void Start(){
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    void GameManager_OnStateChanged(object sender, System.EventArgs e){
        if(GameManager.Instance.IsCountDownToStartActive())
            Show();
        else
            Hide();
    }
    void Update(){
        countdownText.text = Math.Ceiling(GameManager.Instance.GetCountdownToStartTimer()).ToString();
    }

    void Show(){
        gameObject.SetActive(true);
    }
    void Hide(){
        gameObject.SetActive(false);
    }
}
