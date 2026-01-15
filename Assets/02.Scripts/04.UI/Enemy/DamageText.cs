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
    
    private Sequence _sequence;
    
    public override void OnSpawn()
    {
        base.OnSpawn();

        _damageTextUI.alpha = 1f;
        transform.localScale = Vector3.one * _startScale;
    }
    
    public void Setup(Vector3 position, float damage)
    {
        transform.position = Camera.main.WorldToScreenPoint(position + _offset);
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
    }
}
