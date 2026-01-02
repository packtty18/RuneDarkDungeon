using DG.Tweening;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

//임시로 사용할 데미지 피드백
public class DamageFeedback : MonoBehaviour
{
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.5f;

    [SerializeField] private DamageReceiver _receiver;
    [SerializeField] private HurtBox _managingHurtbox;
    [SerializeField] private Renderer _renderer;
    private Color _originalColor;
    private Tween _currentTween;

    private void Awake()
    {
        _receiver = GetComponentInParent<DamageReceiver>();
        _managingHurtbox = GetComponent<HurtBox>();
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
    }

    private void OnEnable()
    {
        _receiver.OnDamagedEvent.Subscribe(HandleDamaged);
    }

    private void OnDisable()
    {
        _receiver.OnDamagedEvent.Unsubscribe(HandleDamaged);
    }

    private void HandleDamaged(HurtBox hurtbox)
    {
        if (hurtbox != _managingHurtbox)
            return;

        PlayFlash();
    }

    [Button("Test Flash")]
    public void PlayFlash()
    {
        _currentTween?.Kill();

        Sequence seq = DOTween.Sequence();

        _renderer.material.color = hitColor;

        seq.AppendInterval(flashDuration);

        Color origin = _originalColor;
        seq.Join(_renderer.material.DOColor(origin, flashDuration));

        _currentTween = seq;
    }
}
