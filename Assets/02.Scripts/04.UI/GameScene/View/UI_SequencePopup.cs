using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_SequencePopup : PUBase
{
    [Header("References")]
    [SerializeField] private Transform _slotParent;
    [SerializeField] private ScrollRect _scrollRect;
    
    [Header("Effect Settings")]
    [SerializeField] private float _startDelay = 1f;
    [SerializeField] private float _interval = 0.1f;
    [SerializeField] private float _scaleTime = 0.3f;
    [SerializeField] private Ease _easeType = Ease.OutBack;

    private Tween _delayedStart;
    private Sequence _popSequence;
    
    public void PlayAnimation()
    {
        _delayedStart?.Kill();
        _delayedStart = DOVirtual.DelayedCall(_startDelay, () =>
        {
            Show();
            PlayPopSequence();
        });
    }

    private void PlayPopSequence()
    {
        _popSequence?.Kill();
        _popSequence = DOTween.Sequence();
        
        _scrollRect.verticalNormalizedPosition = 1f;
        
        int count = 5;
        for (int i = 0; i < _slotParent.childCount; i++)
        {
            Transform child = _slotParent.GetChild(i);
            child.localScale = Vector3.zero;
            float startTime = count * _interval;
            _popSequence.Insert(startTime, child.DOScale(1f, _scaleTime).SetEase(_easeType));
            count++;
        }
        
        _popSequence.OnComplete(() => 
        {
            CursorManager.Instance?.SetCursorLock(false);
        });
    }
}
