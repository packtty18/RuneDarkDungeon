using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamageText : PoolableObject
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _damageTextUI;
    
    [Header("애니메이션 설정")]
    [SerializeField] private float _duration;
    [SerializeField] private float _startScale;
    [SerializeField] private Vector3 _offset;
    
    private Camera _camera;
    private Transform _target;          // 따라다닐 대상
    private Vector3 _lastKnownPosition; // 대상이 죽었을 때 기억할 위치
    private Sequence _sequence;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }
    
    private void UpdatePosition()
    {
        Vector3 targetPosition;

        if (_target != null)
        {
            targetPosition = _target.position;
            _lastKnownPosition = targetPosition;
        }
        else
        {
            targetPosition = _lastKnownPosition;
        }
        transform.position = _camera.WorldToScreenPoint(targetPosition + _offset);
    }
    
    public override void OnSpawn()
    {
        base.OnSpawn();

        _damageTextUI.alpha = 1f;
        transform.localScale = Vector3.one * _startScale;
    }
    
    public void Show(Transform target, float damage)
    {
        _target = target;
        _damageTextUI.SetText("{0}", Mathf.RoundToInt(damage));
        PlayAnimation();
    }
    
    private void PlayAnimation()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        
        _sequence.Join(_damageTextUI.DOFade(0, _duration).SetEase(Ease.InQuad));
        
        _sequence.Join(transform.DOScale(1f, _duration / 2).SetEase(Ease.OutBack));

        _sequence.OnComplete(() => ReturnToPool());
    }
    
    public override void OnDespawn()
    {
        _sequence?.Kill();
        _target = null;
    }
}
