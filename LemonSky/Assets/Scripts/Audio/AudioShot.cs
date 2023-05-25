using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class Shot
{
    public string Name;
    public AudioSource AudioSource;
    [HideInInspector] public float BaseValue;
}

public class AudioShot : MonoBehaviour
{
    public static AudioShot Instance { get; private set; }


    [SerializeField] private List<Shot> _shots;
    private readonly Dictionary<string, Shot> _shotMap = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var shot in _shots)
        {
            shot.BaseValue = shot.AudioSource.volume;
            shot.AudioSource.loop = false;
            _shotMap[shot.Name] = shot;
        }

        ApplyAudioSettings();

        AudioSettings.Instance.OnSettingsChanged += ApplyAudioSettings;
    }

    private void OnDestroy()
    {
        AudioSettings.Instance.OnSettingsChanged -= ApplyAudioSettings;
    }

    public void Play(string shotName)
    {
        if (_shotMap.ContainsKey(shotName))
        {
            _shotMap[shotName].AudioSource.Play();
        }
    }

    public void Play(AudioSource audioSource)
    {
        if (AudioSettings.Instance.UseAudio)
        {
            audioSource.PlayOneShot(audioSource.clip, AudioSettings.Instance.SfxVolumePercent);
        }
    }

    public void PlaySafely(string shotName, float lifeTime = 1.5f)
    {
        if (_shotMap.ContainsKey(shotName))
        {
            PlaySafely(_shotMap[shotName].AudioSource, lifeTime);
        }
    }

    public void PlaySafely(AudioSource audioSource, float lifeTime = 1.5f)
    {
        var go = new GameObject();
        var sas = go.AddComponent<AudioSource>();
        sas.playOnAwake = false;
        sas.loop = false;
        sas.clip = audioSource.clip;
        sas.volume = audioSource.volume;
        sas.pitch = audioSource.pitch;
        sas.spatialBlend = audioSource.spatialBlend;
        sas.reverbZoneMix = audioSource.reverbZoneMix;
        sas.minDistance = audioSource.minDistance;
        sas.maxDistance = audioSource.maxDistance;
        sas.transform.position = audioSource.transform.position;

        if (AudioSettings.Instance.UseAudio)
        {
            sas.PlayOneShot(sas.clip, AudioSettings.Instance.SfxVolumePercent);
        }

        go.AddComponent<AutoDestroyableAudioShot>().LifeTime = lifeTime;
    }

    private void ApplyAudioSettings()
    {
        AudioSettings.Instance.ApplyAudioSorces(
            _shotMap.Values.Select(sh => sh.AudioSource).ToList(),
            _shotMap.Values.Select(sh => sh.BaseValue).ToList(),
            false
        );
    }
}
