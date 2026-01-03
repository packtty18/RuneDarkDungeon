using System;
using UnityEngine;

[Serializable]
public struct SoundData
{
    public AudioClip clip;

    public bool isBgm;
    public bool is3D;

    [Range(0f, 1f)]
    public float volume;

    public float minDistance;
    public float maxDistance;
}

[Serializable]
public class SoundEntry
{
    public string key;
    public SoundData data;
}