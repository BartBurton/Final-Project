using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private Image _fill;
    [SerializeField] private Gradient _gradient;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }


    public float Max { get => _slider.maxValue; }
    public float Current { get => _slider.value; }

    public void SetMax(float value)
    {
        _slider.maxValue = value;
    }

    public void Set(float value)
    {
        _slider.value = value;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
