using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string, HitBox> _hitbox = new Dictionary<string, HitBox>();

    [Button]
    public void Active(string key)
    {
        if(string.IsNullOrEmpty(key))
        {
            Debug.Log($"키가 비어있습니다.");
            return;
        }
        if(!_hitbox.TryGetValue(key, out HitBox hitbox))
        {
            Debug.Log($"{key}에 해당하는 히트박스가 없습니다.");
            return;
        }

        hitbox.Activate();
    }
    [Button]
    public void DeActive(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.Log($"키가 비어있습니다.");
            return;
        }
        if (!_hitbox.TryGetValue(key, out HitBox hitbox))
        {
            Debug.Log($"{key}에 해당하는 히트박스가 없습니다.");
            return;
        }

        hitbox.Deactivate();
    }

    [Button]
    public void ActiveAll()
    {
        foreach (HitBox hitbox in _hitbox.Values)
        {
            hitbox.Activate();
        }
    }
    [Button]
    public void DeActiveAll()
    {
        foreach (HitBox hitbox in _hitbox.Values)
        {
            hitbox.Deactivate();
        }
    }
}
