using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

//팝업UI의 베이스
public abstract class PUBase : UIBase
{
    [Header("애니메이션 설정")]
    [SerializeField] protected bool _animation;
    [SerializeField, ShowIf(nameof(_animation))] protected float _duration = 0.3f;
    [SerializeField, ShowIf(nameof(_animation))] protected Ease _showEase = Ease.OutBack;
    [SerializeField, ShowIf(nameof(_animation))] protected Ease _hideEase = Ease.InBack;
    
    protected Sequence _sequence;
    
    public virtual void Open()
    {
        Init();
        Show();
    }

    public virtual void Close()
    {
        Hide();
    }

    public override void Show()
    {
        if (gameObject.activeSelf) return;
        
        SoundManager.Instance?.Play(ESoundType.UI_Button_Click);
        
        gameObject.SetActive(true);
        if (!_animation) return;
        
        _sequence?.Kill();

        transform.localScale = Vector3.one * 0.8f; 

        _sequence = DOTween.Sequence();
        
        _sequence.Join(transform.DOScale(1f, _duration).SetEase(_showEase));
    }

    public override void Hide()
    {
        if (!gameObject.activeSelf) return;
        
        SoundManager.Instance?.Play(ESoundType.UI_Button_Click);
        
        if (!_animation)
        {
            gameObject.SetActive(false);
            return;
        }
        
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        
        _sequence.Join(transform.DOScale(0.5f, _duration)).SetEase(_hideEase);

        _sequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
