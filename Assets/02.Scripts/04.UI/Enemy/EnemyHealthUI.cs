using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class EnemyHealthUI : UIBase
{
    [Header("Reference")]
    [SerializeField] private EnemyStat _stat;

    [Header("HP Images")]
    [SerializeField] private Image _frontFill;
    [SerializeField] private Image _backFill;

    [Header("Animation Settings")]
    [SerializeField] private float _backDelay = 1f;
    [SerializeField] private float _backLerpDuration = 0.5f;

    [Header("Auto Hide")]
    [SerializeField] private float _autoHideDelay = 3f;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _hitParticle;

    private Tween _backTween;
    private Tween _shakeTween;
    private Tween _hideTween;
    private float _currentHpRatio = 1f;

    private IReadOnlyConsumable<float> _health => _stat != null ? _stat.GetValue(EEnemyConsumableFloat.Health) : null;

    protected override void OnInit()
    {
        base.OnInit();
        _stat = GetComponentInParent<EnemyStat>();
        _frontFill.type = Image.Type.Filled;
        _backFill.type = Image.Type.Filled;

        _frontFill.fillAmount = 1f;
        _backFill.fillAmount = 1f;
    }

    private void OnEnable()
    {
        if(_health != null)
        {
            SetHealth(_health.Current, _health.Max);
            _health.Subscribe(OnHit);
        }
    }

    private void OnDisable()
    {
        _backTween?.Kill();
        _shakeTween?.Kill();
        if (_health != null)
        {
            _health.Unsubscribe(OnHit);
        }
        
    }

    public void SetHealth(float currentHp, float maxHp)
    {
        float targetRatio = Mathf.Clamp01(currentHp / maxHp);

        if (targetRatio >= _currentHpRatio)
        {
            _frontFill.fillAmount = targetRatio;
            _backFill.fillAmount = targetRatio;
            _currentHpRatio = targetRatio;
            return;
        }

        OnHit(targetRatio);
    }

    private void OnHit(float current)
    {
        float targetRatio = current / _health.Max;
        Debug.Log($"[EnemyHealthUI] Hit - TargetRatio: {targetRatio}");

        _currentHpRatio = targetRatio;

        // 즉시 Front 감소
        _frontFill.fillAmount = targetRatio;

        // 파티클
        if (_hitParticle != null)
        {
            _hitParticle.Play();
        }

        // BackFill 지연 감소 (중첩 대응)
        if (_backTween != null && _backTween.IsActive())
        {
            _backTween.Kill();
        }

        _backTween = DOVirtual.DelayedCall(_backDelay, () =>
        {
            _backFill
                .DOFillAmount(targetRatio, _backLerpDuration)
                .SetEase(Ease.OutCubic);
        });

        ResetAutoHide();
    }

    private void ResetAutoHide()
    {
        _hideTween?.Kill();

        _hideTween = DOVirtual.DelayedCall(_autoHideDelay,Hide);
    }

}
