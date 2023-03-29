using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameplayingClockUI : MonoBehaviour
{
    const string Number_Popup = "NumberPopup";
    [SerializeField] Image timerImage;
    [SerializeField] TextMeshProUGUI timerText;
    int previousCountdownNumber;
    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    void Update()
    {
        timerImage.fillAmount = GameManager.Instance.GetGameplayingTimerNormalize();
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetGameplayingTimer());
        timerText.text = countdownNumber.ToString();
        if (previousCountdownNumber != countdownNumber)
            previousCountdownNumber = countdownNumber;
    }


    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }

    void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
            Show();
        else
            Hide();
    }
}
