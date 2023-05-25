using StarterAssets;
using Unity.Netcode;
using UnityEngine;

public class JumpUpPlatform : TriggerPlatform
{
    [SerializeField] private float _upSpeed;
    [SerializeField] private float _upHeight;
    [SerializeField] private float _dontUpDelay = 0;

    private float _currentDontUpDelay = 0;
    private ThirdPersonController _playerController = null;
    private bool _isJumped = false;
    private bool _isSpeedUp = false;

    private AudioNet _audioNet;

    public override void OnNetworkSpawn()
    {
        _audioNet = GetComponent<AudioNet>();
    }

    protected override void HandleAction()
    {
        if (_playerController != null)
        {
            UpPlayer(_playerController);

            _currentDontUpDelay += Time.deltaTime;
            if (_currentDontUpDelay >= _dontUpDelay)
            {
                _isJumped = false;
                _isSpeedUp = false;
                _currentDontUpDelay = 0;
                _playerController = null;
                Destroy(_audioNet.GetAudioSource());
                base.HandleAction();
            }
        }
    }

    protected override void OnPlayerStay(Collider player)
    {
        if (_playerController == null)
        {
            _playerController = player.GetComponent<ThirdPersonController>();
        }
    }

    private void UpPlayer(ThirdPersonController player)
    {
        if (player.OwnerClientId != NetworkManager.Singleton.LocalClientId) return;

        if (!_isSpeedUp)
        {
            player.SetSpeed(_upSpeed);
        }

        if (!_isJumped)
        {
            AudioShot.Instance.Play(_audioNet.GetAudioSource(true));

            player.Jump(_upHeight);
            _isJumped = true;
        }
        else if (player.Grounded)
        {
            _isSpeedUp = true;
        }
    }
}
