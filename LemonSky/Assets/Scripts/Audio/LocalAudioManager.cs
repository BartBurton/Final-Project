using System;
using UnityEngine;

public class LocalAudioManager : MonoBehaviour
{
    [SerializeField] AudioBackground _backgroundMusic;
    [SerializeField] AudioBackground _toxicGooAudio;

    public static LocalAudioManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnStateChanged;
    }

    public void StopMusic()
    {
        _backgroundMusic.Stop();
    }

    public void Stop3DSouds()
    {
        _toxicGooAudio.Stop();
    }

    void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsLocalPlayerReady())
        {
            _toxicGooAudio.Play();
        }
    }
}
