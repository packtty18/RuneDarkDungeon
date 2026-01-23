using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_SequencePopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _slotParent;
    [SerializeField] private ScrollRect _scrollRect;
    
    [Header("Effect Settings")]
    [SerializeField] private float _startDelay = 0.5f;
    [SerializeField] private float _interval = 0.1f;
    [SerializeField] private float _scaleTime = 0.3f;
    [SerializeField] private Ease _easeType = Ease.OutBack;

    private Sequence _sequence;
    
    public void PlayAnimation()
    {
        CursorManager.Instance?.SetCursorLock(false);
        
        _sequence?.Kill();
        _sequence = DOTween.Sequence().SetUpdate(true);
        
        _scrollRect.verticalNormalizedPosition = 1f;
        
        int count = 0;
        for (int i = 0; i < _slotParent.childCount; i++)
        {
            Transform child = _slotParent.GetChild(i);
            child.localScale = Vector3.zero;
            float startTime = _startDelay + count * _interval;
            _sequence.Insert(startTime, child.DOScale(1f, _scaleTime).SetEase(_easeType));
            
            _sequence.InsertCallback(startTime, () => 
            {
                SoundManager.Instance?.Play(ESoundType.Pop);
            });
            
            count++;
        }
    }
}
