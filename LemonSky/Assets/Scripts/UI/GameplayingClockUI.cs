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
    Animator animator;
    [SerializeField]float maxTimer;
    [SerializeField]int previousCountdownNumber;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        maxTimer = GameManager.Instance.GetGameplayingTimer();
        previousCountdownNumber = Mathf.CeilToInt(maxTimer);
        Hide();
    }

    void Update()
    {
        timerImage.fillAmount = GameManager.Instance.GetGameplayingTimerNormalize();
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetGameplayingTimer());
        if(maxTimer < countdownNumber) maxTimer = countdownNumber;
        timerText.text = countdownNumber.ToString();
        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            Debug.Log(countdownNumber / maxTimer);
            if (countdownNumber / maxTimer < 0.1f)
                animator.SetTrigger(Number_Popup);
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

    void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
            Show();
        else
            Hide();
    }
}
