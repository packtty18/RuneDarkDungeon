using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Handles visual hit feedback using All In 1 3D Shader hit effect.
/// Single responsibility: only visual feedback.
/// </summary>
public class EnemyDamageFeedback : MonoBehaviour
{
    [Header("Hit Settings")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitIntensity = 1f;
    [SerializeField] private float flashDuration = 0.2f;

    [Header("References")]
    [SerializeField] private DamageReceiver _receiver;
    [SerializeField] private Renderer _renderer;

    private Tween _currentTween;
    private MaterialPropertyBlock _mpb;

    private static readonly int HitEnableID = Shader.PropertyToID("_HitEnabled");
    private static readonly int HitColorID = Shader.PropertyToID("_HitColor");
    private static readonly int HitAmountID = Shader.PropertyToID("_HitBlend");

    private float _currentHitAmount;

    private void Awake()
    {
        _receiver = GetComponentInChildren<DamageReceiver>();
        _renderer = GetComponentInChildren<Renderer>();

        _mpb = new MaterialPropertyBlock();

        Debug.Log("[DamageFeedback] Initialized (All In 1 3D Shader Hit)");
    }

    private void OnEnable()
    {
        _receiver.OnDamagedEvent.Subscribe(HandleDamaged);
    }

    private void OnDisable()
    {
        _receiver.OnDamagedEvent.Unsubscribe(HandleDamaged);
    }

    private void HandleDamaged()
    {
        PlayHitFlash();
    }

    [Button("Test Hit Flash")]
    public void PlayHitFlash()
    {
        _currentTween?.Kill();

        EnableHit(true);
        SetHitColor(hitColor);

        _currentHitAmount = hitIntensity;
        SetHitAmount(_currentHitAmount);

        // Fade hit amount back to zero
        _currentTween = DOTween.To(
            () => _currentHitAmount,
            value =>
            {
                _currentHitAmount = value;
                SetHitAmount(_currentHitAmount);
            },
            0f,
            flashDuration
        ).OnComplete(() =>
        {
            EnableHit(false);
        });
    }

    private void EnableHit(bool enabled)
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat(HitEnableID, enabled ? 1f : 0f);
        _renderer.SetPropertyBlock(_mpb);
    }

    private void SetHitColor(Color color)
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(HitColorID, color);
        _renderer.SetPropertyBlock(_mpb);
    }

    private void SetHitAmount(float amount)
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat(HitAmountID, amount);
        _renderer.SetPropertyBlock(_mpb);
    }
}
