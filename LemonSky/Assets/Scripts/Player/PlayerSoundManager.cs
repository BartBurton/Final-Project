using UnityEngine;
using Unity.Netcode;

public class PlayerSoundManager : NetworkBehaviour
{
    [SerializeField] private AudioNet _audioNet;

    [Space(10)]
    [SerializeField] private AudioClip _runClip;
    [SerializeField] private AudioClip _sprintClip;
    [SerializeField] private AudioClip _jumpClip;


    private AudioSource _audioNetSource;

    [HideInInspector] public NetworkVariable<bool> isJump = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [HideInInspector] public NetworkVariable<bool> isRun = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [HideInInspector] public NetworkVariable<bool> isSprint = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        _audioNetSource = _audioNet.GetAudioSource(IsLocalPlayer);

        ApplyAudioSettings();

        AudioSettings.Instance.OnSettingsChanged += ApplyAudioSettings;

        isJump.OnValueChanged += IsJumpChanged;
        isRun.OnValueChanged += IsRunChanged;
        isSprint.OnValueChanged += IsSprintChanged;
    }

    public int x = 0;

    void IsJumpChanged(bool prev, bool next)
    {
        if(next)
        {
            x++;
            _audioNetSource.PlayOneShot(_jumpClip);
        }
    }

    void IsRunChanged(bool prev, bool next)
    {
        if(next)
        {
            _audioNetSource.clip = _runClip;
            _audioNetSource.loop = true;
            _audioNetSource.Play();
        }
        else
        {
            _audioNetSource.Stop();
        }
    }

    void IsSprintChanged(bool prev, bool next)
    {
        if (next)
        {
            _audioNetSource.clip = _sprintClip;
            _audioNetSource.loop = true;
            _audioNetSource.Play();
        }
        else
        {
            _audioNetSource.Stop();
        }
    }

    private void ApplyAudioSettings()
    {
        _audioNetSource.volume = AudioSettings.Instance.ApplyVolume(_audioNet.BaseValume, false);
    }
}
