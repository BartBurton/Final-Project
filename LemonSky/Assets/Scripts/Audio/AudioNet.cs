using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioNet : MonoBehaviour
{
    public AudioClip Clip;
    [Range(0, 1)] public float Volume = 1;
    [Range(-3, 3)] public float Pitch = 1;
    public float MinDistance = 1;
    public float MaxDistance = 1;

    private AudioSource _audioSource = null;

    public AudioSource GetAudioSource(bool isLocalPlayer)
    {
        if(_audioSource == null)
        {
            var go = new GameObject();
            go.transform.SetParent(transform, false);

            var sas = go.AddComponent<AudioSource>();
            sas.clip = Clip;
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
