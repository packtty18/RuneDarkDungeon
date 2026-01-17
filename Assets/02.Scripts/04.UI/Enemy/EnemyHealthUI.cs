using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIAutoHide))]
public class EnemyHealthUI : UIBase
{
    [Header("Reference")]
    [SerializeField] private EnemyStat _stat;
    [SerializeField] private EnemyHealth _health;
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

    [ShowInInspector] private IReadOnlyConsumable<float> _healthStat;

    protected override void Awake()
    {
        base.Awake();
        if (_stat == null)
            _stat = GetComponentInParent<EnemyStat>();
        if (_health == null)
            _health = GetComponentInParent<EnemyHealth>();
        if (_autoHider == null)
            _autoHider = GetComponent<UIAutoHide>();
        if (_frontFill != null)
            _frontFill.type = Image.Type.Filled;
        if (_backFill != null)
            _backFill.type = Image.Type.Filled;

        Hide();
    }
    protected override void OnInit()
    {
        _healthStat = _stat.GetValue(EEnemyConsumableFloat.Health);
        if (_healthStat == null)
        {
            Debug.LogWarning("[EnemyHealthUI] Health stat is null!");
            return;
        }


        base.OnInit();

        float ratio = _healthStat.Current / _healthStat.Max;
        SetHealthInstant(ratio);
        _healthStat.Unsubscribe(OnHealthChanged); // 중복 방지
        _healthStat.Subscribe(OnHealthChanged);

        Hide();
        _isInitialized = true;
        
    }

    public void OnOwnerDead()
    {
        CleanupAnimations();
        if (_healthStat != null)
        {
            _healthStat.Unsubscribe(OnHealthChanged);
        }
    }

    private void OnHealthChanged(float currentHp)
    {
        if (_healthStat == null)
            return;

        float targetRatio = currentHp / _healthStat.Max;

        if (_health != null && _health.IsDead)
        {
            Hide();
            return;
        }

        if (targetRatio >= _currentHpRatio)
        {
            SetHealthInstant(targetRatio);
            return;
        }

        Show();
        PlayHitAnimation(targetRatio);
    }

    private void SetHealthInstant(float ratio)
    {
        _currentHpRatio = Mathf.Clamp01(ratio);
        
        if (_frontFill != null)
            _frontFill.fillAmount = _currentHpRatio;
        
        if (_backFill != null)
            _backFill.fillAmount = _currentHpRatio;
    }

    private void PlayHitAnimation(float targetRatio)
    {
        _currentHpRatio = Mathf.Clamp01(targetRatio);

        if (_frontFill != null)
            _frontFill.fillAmount = _currentHpRatio;

        if (_hitParticle != null)
        {
            _hitParticle.Play();
        }

        CleanupAnimations();
        
        _backTween = DOVirtual.DelayedCall(_backDelay, () =>
        {
            if (_backFill != null)
            {
                _backFill
                    .DOFillAmount(_currentHpRatio, _backLerpDuration)
                    .SetEase(Ease.OutCubic);
            }
        });

        // AutoHide 타이머 리셋 (3초 후 자동 숨김)
        if (_autoHider != null)
        {
            _autoHider.ResetTimer();
        }
    }

    private void CleanupAnimations()
    {
        _backTween?.Kill();
        _backTween = null;

        // 파티클 정지
        if (_hitParticle != null && _hitParticle.isPlaying)
        {
            _hitParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public override void Hide()
    {
        CleanupAnimations();
        base.Hide();
    }
}
