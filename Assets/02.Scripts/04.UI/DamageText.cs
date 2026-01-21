using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamageText : PoolableObject
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _damageTextUI;
    
    [Header("애니메이션 설정")]
    [SerializeField] private float _floatDistance;
    [SerializeField] private float _duration;
    [SerializeField] private float _startScale;
    [SerializeField] private Vector3 _offset;
    
    private Camera _camera;
    private Vector3 _startPosition;
    private Sequence _sequence;
    private float _currentFloatY;
    
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
        Vector3 screenPosition = _camera.WorldToScreenPoint(_startPosition + _offset);
        screenPosition.y += _currentFloatY;
        transform.position = screenPosition;
    }
    
    public override void OnSpawn()
    {
        base.OnSpawn();

        _damageTextUI.alpha = 1f;
        transform.localScale = Vector3.one * _startScale;
        _currentFloatY = 0f;
    }
    
    public void Show(Vector3 position, float damage)
    {
        _startPosition = position;
        _damageTextUI.SetText("{0}", Mathf.RoundToInt(damage));
        PlayAnimation();
    }
    
    private void PlayAnimation()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        
        _sequence.Join(DOTween.To(()=> _currentFloatY, x=> _currentFloatY = x, _floatDistance, _duration)
            .SetEase(Ease.OutQuart));
        
        _sequence.Join(_damageTextUI.DOFade(0, _duration).SetEase(Ease.InQuad));
        
        _sequence.Join(transform.DOScale(1f, _duration / 2).SetEase(Ease.OutBack));

        _sequence.OnComplete(() => ReturnToPool());
    }
    
    public override void OnDespawn()
    {
        _sequence?.Kill();
    }
}
