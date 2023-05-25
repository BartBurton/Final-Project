using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioBackground : MonoBehaviour
{
    [SerializeField] private bool _playAll = false;
    [SerializeField] private bool _playOnAwake = true;
    [SerializeField] private bool _isMusic = true;
    [SerializeField] private List<AudioSource> _musicList;

    private List<float> _baseValumes;

    private int _nowPlayingIndex = -1;
    private List<int> _playedMusics = new();

    private void Start()
    {
        _baseValumes = _musicList.Select(m => m.volume).ToList();

        ApplyAudioSettings();

        AudioSettings.Instance.OnSettingsChanged += ApplyAudioSettings;

        if (_playOnAwake)
        {
            Play();
        }
    }

    private void OnDestroy()
    {
        AudioSettings.Instance.OnSettingsChanged -= ApplyAudioSettings;
    }

    public void Play()
    {
        if (_musicList.Count == 0) return;

        if(_playAll)
        {
            foreach (var music in _musicList)
            {
                music.Play();
            }

            return;
        }

        var notPlayed = _musicList.Select((e, i) => i).Except(_playedMusics).ToList();

        if (notPlayed.Count == 0)
        {
            _playedMusics.Clear();
            notPlayed = _musicList.Select((e, i) => i).ToList();
        }

        _nowPlayingIndex = notPlayed[new System.Random().Next(0, notPlayed.Count)];
        _playedMusics.Add(_nowPlayingIndex);
        _musicList[_nowPlayingIndex].Play();

        Invoke(nameof(this.PlayNext), _musicList[_nowPlayingIndex].clip.length);
    }

    public void Stop()
    {
        if (_playAll)
        {
            foreach (var music in _musicList)
            {
                music.Stop();
            }

            return;
        }

        if (_nowPlayingIndex != -1 && _musicList.Count > _nowPlayingIndex)
        {
            _musicList[_nowPlayingIndex].Stop();
        }
    }

    private void PlayNext()
    {
        Stop();
        Play();
    }

    private void ApplyAudioSettings()
    {
        AudioSettings.Instance.ApplyAudioSorces(_musicList, _baseValumes, _isMusic);
    }
}
