using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIAutoHide))]
public class EnemyHealthUI : UIBase
{
    [Header("Reference")]
    [SerializeField] private EnemyStat _stat;
    [SerializeField] private UIAutoHide _autoHider;

    [Header("HP Images")]
    [SerializeField] private Image _frontFill;
    [SerializeField] private Image _backFill;

    [Header("Animation Settings")]
    [SerializeField] private float _backDelay = 1f;
    [SerializeField] private float _backLerpDuration = 0.5f;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _hitParticle;

    private Tween _backTween;
    private float _currentHpRatio = 1f;

    private IReadOnlyConsumable<float> _health => _stat != null ? _stat.GetValue(EEnemyConsumableFloat.Health): null;


    protected override void OnInit()
    {
        base.OnInit();

        _stat = GetComponentInParent<EnemyStat>();
        _autoHider = GetComponent<UIAutoHide>();

        _frontFill.type = Image.Type.Filled;
        _backFill.type = Image.Type.Filled;

        // 초기값은 항상 풀 상태
        SetHealthInstant(1f);
    }

    private void OnEnable()
    {
        if (_health == null)
            return;

        // 풀링 복귀 시 즉시 동기화
        SetHealth(_health.Current, _health.Max);
        _health.Subscribe(OnHealthChanged);
    }

    private void OnDisable()
    {
        _backTween?.Kill();
        _backTween = null;

        _health?.Unsubscribe(OnHealthChanged);
    }

    public void SetHealth(float currentHp, float maxHp)
    {
        float ratio = Mathf.Clamp01(currentHp / maxHp);
        SetHealthInstant(ratio);
    }

    private void OnHealthChanged(float currentHp)
    {
        float targetRatio = currentHp / _health.Max;

        if (targetRatio >= _currentHpRatio)
        {
            // 회복 or 동일 → 즉시 반영
            SetHealthInstant(targetRatio);
            return;
        }

        PlayHitAnimation(targetRatio);
    }

    private void SetHealthInstant(float ratio)
    {
        _currentHpRatio = ratio;
        _frontFill.fillAmount = ratio;
        _backFill.fillAmount = ratio;
    }

    private void PlayHitAnimation(float targetRatio)
    {
        _currentHpRatio = targetRatio;

        // Front 즉시 감소
        _frontFill.fillAmount = targetRatio;

        // VFX
        if (_hitParticle != null)
        {
            _hitParticle?.Play();
        }

        // BackFill 지연 감소
        _backTween?.Kill();
        _backTween = DOVirtual.DelayedCall(_backDelay, () =>
        {
            _backFill
                .DOFillAmount(targetRatio, _backLerpDuration)
                .SetEase(Ease.OutCubic);
        });

        // AutoHide 리셋
        _autoHider.ResetTimer();
    }
}
