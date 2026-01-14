using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyDamageFeedback : MonoBehaviour
{
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    [SerializeField] private DamageReceiver _receiver;
    [SerializeField] private Renderer _renderer;

    private Color _originalColor;
    private Color _currentColor;

    private Tween _currentTween;
    private MaterialPropertyBlock _mpb;

    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        _receiver = GetComponentInChildren<DamageReceiver>();
        _renderer = GetComponentInChildren<Renderer>();

        _mpb = new MaterialPropertyBlock();

        if (_renderer.sharedMaterial.HasProperty(BaseColorID))
        {
            _originalColor = _renderer.sharedMaterial.GetColor(BaseColorID);
        }
        else
        {
            _originalColor = Color.white;
            Debug.LogWarning("[DamageFeedback] Material has no _BaseColor property.", this);
        }

        _currentColor = _originalColor;
    }


    private void OnEnable()
    {
        _receiver.OnDamagedEvent.Subscribe(HandleDamaged);
    }

    private void OnDisable()
    {
        _receiver.OnDamagedEvent.Unsubscribe(HandleDamaged);
    }

    private void HandleDamaged()
    {
        PlayFlash();
    }

    [Button("Test Flash")]
    public void PlayFlash()
    {
        _currentTween?.Kill();

        _currentColor = hitColor;
        SetColor(_currentColor);

        _currentTween = DOTween.To(
            () => _currentColor,
            color =>
            {
                _currentColor = color;
                SetColor(_currentColor);
            },
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
