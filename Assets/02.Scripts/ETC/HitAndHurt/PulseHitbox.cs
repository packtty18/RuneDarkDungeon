using UnityEngine;
using Sirenix.OdinInspector;
public class PulseHitbox : MonoBehaviour
{
    [Title("References")]
    [SerializeField] private HitboxController _hitboxController;
    [SerializeField] private string _hitboxKey = "Main";

    [Title("Damage")]
    [SerializeField] private float _damage = 10f;

    [Title("Life Time")]
    [SerializeField] private float _lifeTime = 3f;

    [Title("Pulse Settings")]
    [Tooltip("Time hitbox stays active")]
    [SerializeField] private float _activeTime = 0.2f;

    [Tooltip("Time hitbox stays inactive")]
    [SerializeField] private float _inactiveTime = 0.5f;

    [Tooltip("If true, pulse only once (Explosion)")]
    [SerializeField] private bool _singlePulse = false;

    private float _lifeTimer;
    private float _pulseTimer;
    private bool _isActive;

    private void OnEnable()
    {
        _lifeTimer = _lifeTime;
        _pulseTimer = 0f;
        _isActive = false;

        Debug.Log("[HitboxPulseRunner] Started");
    }

    private void Update()
    {
        UpdateLifeTime();
        UpdatePulse();
    }

    private void UpdateLifeTime()
    {
        _lifeTimer -= Time.deltaTime;

        if (_lifeTimer <= 0f)
        {
            StopAll();
            Debug.Log("[HitboxPulseRunner] Life ended");
            Destroy(gameObject);
        }
    }

    private void UpdatePulse()
    {
        _pulseTimer -= Time.deltaTime;

        if (_pulseTimer > 0f)
            return;

        if (_isActive)
        {
            Deactivate();

            if (_singlePulse)
                return;

            _pulseTimer = _inactiveTime;
        }
        else
        {
            Activate();
            _pulseTimer = _activeTime;
        }
    }

    private void Activate()
    {
        _isActive = true;
        _hitboxController.Activate(_hitboxKey, _damage);
        Debug.Log("[HitboxPulseRunner] Hitbox Activated");
    }

    private void Deactivate()
    {
        _isActive = false;
        _hitboxController.Deactivate(_hitboxKey);
        Debug.Log("[HitboxPulseRunner] Hitbox Deactivated");
    }

    private void StopAll()
    {
        _hitboxController.Deactivate(_hitboxKey);
        _isActive = false;
    }
}
