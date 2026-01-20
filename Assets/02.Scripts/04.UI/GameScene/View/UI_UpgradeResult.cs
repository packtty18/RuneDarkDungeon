using UnityEngine;
using DG.Tweening;

public class UI_UpgradeResult : PUBase
{
    [SerializeField] private float _displayDuration = 0.2f;
    
    public override void Show()
    {
        gameObject.SetActive(true);
        
        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        transform.localScale = Vector3.zero;
        
        _sequence.Join(transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack));
        
        _sequence.AppendInterval(_displayDuration);
        
        _sequence.Append(transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));

        _sequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
