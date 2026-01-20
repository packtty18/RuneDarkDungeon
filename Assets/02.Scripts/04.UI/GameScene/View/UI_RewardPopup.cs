using UnityEngine;
using DG.Tweening;

public class UI_RewardPopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _slotParent;

    [Header("Effect Settings")]
    [SerializeField] private float _startDelay = 2f;
    [SerializeField] private float _interval = 0.1f;
    [SerializeField] private float _scaleTime = 0.3f;
    [SerializeField] private Ease _easeType = Ease.OutBack;

    Sequence _sequence;
    
    public void PlayRewardAnimation()
    {
        CursorManager.Instance?.SetCursorLock(false);
        PlayPopSequence();
    }

    private void PlayPopSequence()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        
        gameObject.SetActive(true);
        
        int activeChildIndex = 0;

        for (int i = 0; i < _slotParent.childCount; i++)
        {
            Transform child = _slotParent.GetChild(i);
            child.localScale = Vector3.zero;
            float startTime = activeChildIndex * _interval;
            _sequence.Insert(startTime, child.DOScale(1f, _scaleTime).SetEase(_easeType));
            activeChildIndex++;
        }
    }
}
