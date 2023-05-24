using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
#if UNITY_EDITOR
    private static AudioSettings _instance;
    public static AudioSettings Instance
    {
        get
        {
            if(_instance == null)
            {
                var go = new GameObject(nameof(AudioSettings));
                _instance = go.AddComponent<AudioSettings>();
            }

            return _instance;
        }
        set { _instance = value; }
    }
#else
    public static AudioSettings Instance { get; private set; }
#endif

    [SerializeField] private bool _useAudio = true;
    public bool UseAudio
    {
        get => _useAudio;
        set
        {
            _useAudio = value;
            OnSettingsChanged?.Invoke();
        }
    }

    [SerializeField][Range(0, 1)] private float _musicVolumePercent = 1;
    public float MusicVolumePercent
    {
        get => _musicVolumePercent;
        set
        {
            _musicVolumePercent = GetVolume(value);
            OnSettingsChanged?.Invoke();
        }
    }

    [SerializeField][Range(0, 1)] private float _sfxVolumePercent = 1;
    public float SfxVolumePercent
    {
        get => _sfxVolumePercent;
        set
        {
            _sfxVolumePercent = GetVolume(value);
            OnSettingsChanged?.Invoke();
        }
    }

    public event Action OnSettingsChanged;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ApplyAudioSorces(List<AudioSource> audioSorces, List<float> baseValumes, bool isMusic = true)
    {
        for (int i = 0; i < baseValumes.Count; i++)
        {
            audioSorces[i].volume = ApplyVolume(baseValumes[i], isMusic);
        }
    }

    public float ApplyVolume(float value, bool isMusic = true)
    {
        if (UseAudio)
        {
            if (isMusic)
            {
                return value - (value - value * MusicVolumePercent);
            }
            else
            {
                return value - (value - value * SfxVolumePercent);
            }
        }

        return 0;
    }

    private float GetVolume(float value) => value > 1 ? 1 : value < 0 ? 0 : value;
}

