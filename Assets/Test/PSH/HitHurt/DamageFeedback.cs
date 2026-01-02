using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

// 임시로 사용할 데미지 피드백
public class DamageFeedback : MonoBehaviour
{
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.5f;

    [SerializeField] private DamageReceiver _receiver;
    [SerializeField] private HurtBox _managingHurtbox;
    [SerializeField] private Renderer _renderer;

    private Color _originalColor;
    private Tween _currentTween;

    private MaterialPropertyBlock _mpb;
    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        _receiver = GetComponentInParent<DamageReceiver>();
        _managingHurtbox = GetComponent<HurtBox>();
        _renderer = GetComponent<Renderer>();

        _mpb = new MaterialPropertyBlock();

        _renderer.GetPropertyBlock(_mpb);
        _originalColor = _mpb.GetColor(BaseColorID);
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
        {
            return;
        }

        PlayFlash();
    }

    [Button("Test Flash")]
    public void PlayFlash()
    {
        _currentTween?.Kill();

        // 즉시 피격 색상 적용
        SetColor(hitColor);

        _currentTween = DOTween.To(
            () => hitColor,
            color => SetColor(color),
            _originalColor,
            flashDuration
        );
    }

    private void SetColor(Color color)
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(BaseColorID, color);
        _renderer.SetPropertyBlock(_mpb);
    }
}
