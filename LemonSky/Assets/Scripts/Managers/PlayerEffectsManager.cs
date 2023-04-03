using Unity.Netcode;
using UnityEngine;

public enum PlayerEffectState
{
    Default = 0,
    Flickering = 1,
    Death = 2,
}

public class PlayerEffectsManager : NetworkBehaviour
{
    private bool _hasSkin = false;
    private GameObject _modelSkin;

    private PlayerEffectState _effectState = PlayerEffectState.Default;
    private float _effectDuration = 0f;
    private float _currentEffectDuration = 0f;

    private void Update()
    {
        if (_hasSkin && _effectState != PlayerEffectState.Default)
        {
            switch (_effectState)
            {
                case PlayerEffectState.Flickering:
                    Flickering();
                    break;
                case PlayerEffectState.Death:
                    Death();
                    break;
                default:
                    break;
            }
        }
    }

    public void SetEffectState(PlayerEffectState state, float effectDuration)
    {
        _effectState = state;
        _effectDuration = effectDuration;
        _currentEffectDuration = 0f;
    }

    public void SetModelSkin(GameObject skin)
    {
        _modelSkin = skin.transform.GetChild(1).gameObject;
        _hasSkin = true;
    }

    private void Flickering()
    {
        _modelSkin.GetComponent<SkinnedMeshRenderer>().enabled = true;

        _currentEffectDuration += Time.deltaTime;
        if (_currentEffectDuration < _effectDuration)
        {
            if ((int)(_currentEffectDuration * 100) % 500 < 250)
            {
                _modelSkin.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
            else
            {
                _modelSkin.GetComponent<SkinnedMeshRenderer>().enabled = true;
            }
        }
        else
        {
            SetEffectState(PlayerEffectState.Default, 0);
        }
    }

    private void Death()
    {

    }
}
