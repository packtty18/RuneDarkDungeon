using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : UIBase
{
    [Header("Reference")]
    [SerializeField] protected EnemyStat _stat;
    [SerializeField] protected EnemyHealth _health;
    [SerializeField] protected UIAutoHide _autoHider;

    [Header("HP Images")]
    [SerializeField] protected Image _frontFill;
    [SerializeField] protected Image _backFill;


    [Header("Animation Settings")]
    [SerializeField] protected float _backDelay = 1f;
    [SerializeField] protected float _backLerpDuration = 0.5f;

    [Header("VFX")]
    [SerializeField] protected ParticleSystem _hitParticle;

    protected Tween _backTween;
    protected float _currentHpRatio = 1f;

    [ShowInInspector] protected IReadOnlyConsumable<float> _healthStat;
    
    //생성됬을때
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

    //적이 Init되었을때
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

    //적이 사망했을때
    public void OnOwnerDead()
    {
        Hide();
        if (_healthStat != null)
        {
            _healthStat.Unsubscribe(OnHealthChanged);
        }
    }


    //적의 HP에 변화가 있을때
    protected void OnHealthChanged(float currentHp)
    {
        if (_healthStat == null || _healthStat.IsFull())
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

    protected void SetHealthInstant(float ratio)
    {
        _currentHpRatio = Mathf.Clamp01(ratio);
        
        if (_frontFill != null)
            _frontFill.fillAmount = _currentHpRatio;
        
        if (_backFill != null)
            _backFill.fillAmount = _currentHpRatio;
    }

    protected void PlayHitAnimation(float targetRatio)
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

    protected void CleanupAnimations()
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
