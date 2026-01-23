using System;
using System.Collections;
using UnityEngine;

//추후 풀링 기능 추가할것.
[RequireComponent(typeof(AudioSource))]
public class SoundObject : PoolableObject
{
    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void Play(SoundData data)
    {
        if (data.clip == null)
        {
            Debug.LogWarning("Play called with a null audio clip.");
            ReturnToPool();
            return;
        }
        _audio.clip = data.clip;
        _audio.volume = data.volume;
        _audio.spatialBlend = data.is3d ? 1f : 0f;
        if (data.is3d)
        {
            _audio.minDistance = Mathf.Max(0.1f, data.minDistance);
            _audio.maxDistance = Mathf.Max(_audio.minDistance + 0.1f, data.maxDistance);
        }
        
        _audio.Play();

        ReturnToPoolAfter(data.clip.length);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Stop()
    {
        _audio.Stop();
        _audio.clip = null;

        ReturnToPool();
    }

}
