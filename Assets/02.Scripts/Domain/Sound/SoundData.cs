using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public struct SoundData
{
    public AudioClip clip;
    public bool isBgm;
    [Range(0f, 1f)]
    public float volume;

    [HideIf(nameof(isBgm))]
    public float minDistance;
    [HideIf(nameof(isBgm))]
    public float maxDistance;
}

[Serializable]
public class SoundEntry
{
    public string key;
    public SoundData data;
}