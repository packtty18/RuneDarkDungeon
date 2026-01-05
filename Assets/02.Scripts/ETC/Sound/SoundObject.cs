using System;
using System.Collections;
using UnityEngine;

//추후 풀링 기능 추가할것.
[RequireComponent(typeof(AudioSource))]
public class SoundObject : PoolableObject
{
    private AudioSource _audio;

    private bool _isBgm = false;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void Play(SoundData data, Vector3 position)
    {
        if (data.clip == null)
        {
            Debug.LogWarning("Play called with a null audio clip.");
            if (!_isBgm)
            {
                ReturnToPool();
            }
            return;
        }

        transform.position = position;

        _audio.clip = data.clip;
        _audio.volume = data.volume;
        _audio.spatialBlend = data.isBgm ? 0f : (data.is3D ? 1f : 0f);
        _audio.minDistance = Mathf.Max(0.1f, data.minDistance);
        _audio.maxDistance = Mathf.Max(_audio.minDistance + 0.1f, data.maxDistance);
        _audio.loop = data.isBgm;
        _isBgm = data.isBgm;
        _audio.Play();

        if (!_isBgm)
        {
            ReturnToPoolAfter(data.clip.length);
        }
    }

    public void Stop()
    {
        _audio.Stop();
        _audio.clip = null;

        if(!_isBgm)
        {
            ReturnToPool();
        }
    }

}
