using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TemporaryPlatform : TriggerPlatform
{
    [SerializeField] private float _invisibilityDelay = 0;
    private float _currentInvisibilityDelay = 0;

    protected override void HandleAction()
    {
        if (_currentInvisibilityDelay < _invisibilityDelay)
        {
            _currentInvisibilityDelay += Time.deltaTime;
            gameObject.SetActive(false);
        }
        else
        {
            _currentInvisibilityDelay = 0;
            gameObject.SetActive(true);
            base.HandleAction();
        }
    }
}
