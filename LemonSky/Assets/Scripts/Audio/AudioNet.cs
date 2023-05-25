using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioNet : MonoBehaviour
{
    public AudioClip Clip;
    public bool Loop = false;
    [Range(0, 1)] public float Volume = 1;
    [Range(-3, 3)] public float Pitch = 1;
    public float MinDistance = 1;
    public float MaxDistance = 1;

    [HideInInspector] public float BaseValume = 1;

    private AudioSource _audioSource = null;

    private void Start()
    {
        BaseValume = Volume;
    }

    public AudioSource GetAudioSource(bool isLocalPlayer = true)
    {
        if(_audioSource == null)
        {
            var go = new GameObject();
            go.transform.SetParent(transform, false);

            var sas = go.AddComponent<AudioSource>();
            sas.playOnAwake = false;
            sas.clip = Clip;
            sas.loop = Loop;
            sas.volume = Volume;
            sas.pitch = Pitch;
            sas.spatialBlend = isLocalPlayer ? 0 : 1;
            sas.minDistance = MinDistance;
            sas.maxDistance = MaxDistance;
            sas.transform.position = transform.position;

            _audioSource = sas;
        }

        return _audioSource;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new(0.0f, 1.0f, 0.0f, 0.2f);

        Gizmos.DrawSphere(
            transform.position,
            MinDistance
        );


        Gizmos.color = new(1.0f, 0.0f, 0.0f, 0.2f);

        Gizmos.DrawSphere(
            transform.position,
            MaxDistance
        );
    }
}
