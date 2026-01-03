using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Sound/SoundDatabase")]
public class SoundDatabaseSO : ScriptableObject
{
    [SerializeField] private List<SoundEntry> _entries;

    private Dictionary<string, SoundData> _lookup;

    public void Init()
    {
        _lookup = new Dictionary<string, SoundData>();

        foreach (SoundEntry entry in _entries)
        {
            if (string.IsNullOrEmpty(entry.key))
            {
                continue;
            }

            if (_lookup.ContainsKey(entry.key))
            {
                Debug.LogWarning($"[SoundDatabase] Duplicate key: {entry.key}");
                continue;
            }

            _lookup.Add(entry.key, entry.data);
        }
    }

    public bool TryGet(ESoundType key, out SoundData data)
    {
        if (_lookup == null)
        {
            Init();
        }
        
        if(ResolveKey(key, out var resolvedKey) == false)
        {
            data = default;
            return false;
        }

        return _lookup.TryGetValue(resolvedKey, out data);
    }

    //Enum을 string으로 변환
    private bool ResolveKey(ESoundType key, out string resolvedKey)
    {
        resolvedKey = key.ToString();
        return _lookup.ContainsKey(resolvedKey);
    }

    #region Debug용
    [Button("Validate Enum Keys")]
    private void ValidateEnumKeys()
    {
        if (_lookup == null)
        {
            Init();
        }

        foreach (ESoundType type in System.Enum.GetValues(typeof(ESoundType)))
        {
            if (type == ESoundType.None)
            {
                continue;
            }

            string key = type.ToString();

            if (_lookup.ContainsKey(key) == false)
            {
                Debug.LogWarning($"[SoundDatabase] Enum not registered in database: {key}");
            }
        }

        Debug.Log("[SoundDatabase] Enum validation finished.");
    }
    [Button("Add Missing Enum Keys")]
    private void AddMissingEnumKeys()
    {
        if (_entries == null)
        {
            _entries = new List<SoundEntry>();
        }

        HashSet<string> existingKeys = new HashSet<string>();

        foreach (SoundEntry entry in _entries)
        {
            if (string.IsNullOrEmpty(entry.key) == false)
            {
                existingKeys.Add(entry.key);
            }
        }

        int addedCount = 0;

        foreach (ESoundType type in System.Enum.GetValues(typeof(ESoundType)))
        {
            if (type == ESoundType.None)
            {
                continue;
            }

            string key = type.ToString();

            if (existingKeys.Contains(key) == false)
            {
                SoundEntry newEntry = new SoundEntry();
                newEntry.key = key;
                newEntry.data = default;

                _entries.Add(newEntry);
                addedCount++;
            }
        }

        Debug.Log($"[SoundDatabase] Added {addedCount} missing enum keys.");
    }

    [Button("Validate Missing AudioClips")]
    private void ValidateMissingAudioClips()
    {
        int missingCount = 0;

        foreach (SoundEntry entry in _entries)
        {
            if (string.IsNullOrEmpty(entry.key))
            {
                continue;
            }

            if (entry.data.clip == null)
            {
                Debug.LogWarning($"[SoundDatabase] Missing AudioClip for key: {entry.key}");
                missingCount++;
            }
        }

        Debug.Log($"[SoundDatabase] AudioClip validation finished. Missing count: {missingCount}");
    }
    #endregion
}
