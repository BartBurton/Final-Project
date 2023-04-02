using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Serializable]
    private class SpriteConfig
    {
        public Sprite Sptite;
        public int StartingPercent;
    }

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textContainer;
    [SerializeField] private SpriteConfig _timerLarge;
    [SerializeField] private SpriteConfig _timerMiddle;
    [SerializeField] private SpriteConfig _timerSmall;

    private float _timeToMiddle;
    private float _timeToSmall;
    private float _timeToEnd;

    // Start is called before the first frame update
    void Start()
    {
        _image.sprite = _timerLarge.Sptite;
        _timeToMiddle = GameManager.Instance.GetGamePlayingTimerMax() * (_timerMiddle.StartingPercent / 100f);
        _timeToSmall = GameManager.Instance.GetGamePlayingTimerMax() * (_timerSmall.StartingPercent / 100f);
        _timeToEnd = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _image.fillAmount = GameManager.Instance.GetGameplayingTimer() / GameManager.Instance.GetGamePlayingTimerMax();
        int countDownNumber = Mathf.CeilToInt(GameManager.Instance.GetGameplayingTimer());

        if (countDownNumber < 60)
        {
            _textContainer.text = countDownNumber.ToString() + 'c';
        }
        else
        {
            _textContainer.text = (Mathf.Floor(countDownNumber / 60f) + 1).ToString() + 'м';
        }

    }

    void FixedUpdate()
    {
        if (GameManager.Instance.GetGameplayingTimer() > _timeToMiddle)
        {
            _image.sprite = _timerLarge.Sptite;
        }
        else if (GameManager.Instance.GetGameplayingTimer() > _timeToSmall)
        {
            _image.sprite = _timerMiddle.Sptite;
        }
        else if (GameManager.Instance.GetGameplayingTimer() > _timeToEnd)
        {
            _image.sprite = _timerSmall.Sptite;
        }
    }
}
