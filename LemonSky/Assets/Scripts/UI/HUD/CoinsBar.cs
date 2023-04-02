using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textContainer;
    [SerializeField] private RectTransform _rectTransform;

    [SerializeField] private float _upScaleFactorPart;
    [SerializeField] private int _baseDigitCount;

    private float _upScaleFactor = 1;
    private int _prevDigitCount = 0;

    private void Start()
    {
        _upScaleFactor = _rectTransform.sizeDelta.x / _upScaleFactorPart;
        _prevDigitCount = _baseDigitCount;
    }

    private void UpScaleRectTransform(float value)
    {
        _rectTransform.sizeDelta = new Vector2(
            _rectTransform.sizeDelta.x + value,
            _rectTransform.sizeDelta.y
        );
    }

    public void SetCount(int count)
    {
        string strCount = count.ToString();

        if (strCount.Length > _prevDigitCount)
        {
            UpScaleRectTransform(_upScaleFactor * (strCount.Length - _prevDigitCount));
            _prevDigitCount = strCount.Length;
        }
        else if (strCount.Length < _prevDigitCount)
        {
            if (strCount.Length < _baseDigitCount)
            {
                UpScaleRectTransform(-_upScaleFactor * (_prevDigitCount - _baseDigitCount));
                _prevDigitCount = _baseDigitCount;
            }
            else
            {
                UpScaleRectTransform(-_upScaleFactor * (_prevDigitCount - strCount.Length));
                _prevDigitCount = strCount.Length;
            }
        }

        _textContainer.text = strCount;
    }
}
