using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

// Controls activation and deactivation of grouped hitboxes
public class HitboxController : SerializedMonoBehaviour
{
    [SerializeField]
    private Dictionary<string, List<HitBox>> _hitboxes = new();

    [Button]
    public void Activate(string key)
    {
        if (!TryGetHitboxes(key, out var hitboxes))
        {
            return;
        }
        SetActive(hitboxes, true);
    }

    [Button]
    public void Deactivate(string key)
    {
        if (!TryGetHitboxes(key, out var hitboxes))
        {
            return;
        }
        SetActive(hitboxes, false);
    }

    [Button]
    public void ActivateAll()
    {
        foreach (var hitboxes in _hitboxes.Values)
        {
            SetActive(hitboxes, true);
        }
    }

    [Button]
    public void DeactivateAll()
    {
        foreach (var hitboxes in _hitboxes.Values)
        {
            SetActive(hitboxes, false);
        }
    }

    private bool TryGetHitboxes(string key, out List<HitBox> hitboxes)
    {
        hitboxes = null;

        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning("[HitboxController] Key is null or empty.");
            return false;
        }

        if (!_hitboxes.TryGetValue(key, out hitboxes))
        {
            Debug.LogWarning($"[HitboxController] No hitboxes found for key: {key}");
            return false;
        }

        return true;
    }

    private void SetActive(List<HitBox> hitboxes, bool active, float activeDamage = 0)
    {
        foreach (HitBox hitbox in hitboxes)
        {
            if (active)
            {
                hitbox.Activate(activeDamage);
            }
            else
            {
                hitbox.Deactivate();
            }
        }
    }
}
