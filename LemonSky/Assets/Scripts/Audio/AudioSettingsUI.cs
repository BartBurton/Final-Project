using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private bool _isOpen = false;
    [SerializeField] private Button _showButton;

    [Space(10)]
    [SerializeField] private GameObject _audioSettingsPanel;

    [Space(10)]
    [SerializeField] private Button _audioSwitchButton;
    [SerializeField] private TextMeshProUGUI _audioSwitchButtonText;
    [SerializeField] private TextMeshProUGUI _audioStatusText;
    [SerializeField] private Color _audioOnColor;
    [SerializeField] private Color _audioOffColor;

    [Space(10)]
    [SerializeField] private Slider _musicSlider;

    [Space(10)]
    [SerializeField] private Slider _sfxSlider;


    void Start()
    {
        ApplyOpen(_isOpen);

        _showButton.onClick.AddListener(() => {
            if (_isOpen)
            {
                AudioShot.Instance.Play("second");
            }
            else
            {
                AudioShot.Instance.Play("main");
            }
            ApplyOpen(!_isOpen); 
        });

        ApplyAudioSettings();

        _musicSlider.value = AudioSettings.Instance.MusicVolumePercent;
        _sfxSlider.value = AudioSettings.Instance.SfxVolumePercent;

        _audioSwitchButton.onClick.AddListener(() => { AudioSettings.Instance.UseAudio = !AudioSettings.Instance.UseAudio; });
        _musicSlider.onValueChanged.AddListener((value) => { AudioSettings.Instance.MusicVolumePercent = value; });
        _sfxSlider.onValueChanged.AddListener((value) => { AudioSettings.Instance.SfxVolumePercent = value; });

        AudioSettings.Instance.OnSettingsChanged += ApplyAudioSettings;
    }

    private void OnDestroy()
    {
        AudioSettings.Instance.OnSettingsChanged -= ApplyAudioSettings;
    }

    private void ApplyOpen(bool isOpen)
    {
        _audioSettingsPanel.SetActive(isOpen);
        _isOpen = isOpen;
    }

    private void ApplyAudioSettings()
    {
        if (AudioSettings.Instance.UseAudio)
        {
            _audioSwitchButton.GetComponent<Image>().color = _audioOffColor;
            _audioSwitchButtonText.text = "ВЫКЛ";
            _audioStatusText.text = "Включен";
        }
        else
        {
            _audioSwitchButton.GetComponent<Image>().color = _audioOnColor;
            _audioSwitchButtonText.text = "ВКЛ";
            _audioStatusText.text = "Выключен";
        }
    }
}
