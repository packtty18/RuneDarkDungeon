using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public struct SoundData
{
    public AudioClip clip;
    public bool isBgm;
    [HideIf(nameof(isBgm))]
    public bool is3d;
    [Range(0f, 1f)]
    public float volume;
    [HideIf(nameof(HideDistance))]
    public float minDistance;
    [HideIf(nameof(HideDistance))]
    public float maxDistance;

    public bool HideDistance()
    {
        return isBgm || !is3d;
}

}


[Serializable]
public class SoundEntry
{
    public string key;
    public SoundData data;
}