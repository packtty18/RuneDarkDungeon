using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Controls all shader-based visual feedback using All In 1 3D Shader.
/// Single responsibility: shader parameter control only.
/// </summary>
public class EnemyShaderFeedback : MonoBehaviour
{
    private static readonly int FadeID = Shader.PropertyToID("_FadeAmount");
    private static readonly int RimEnableID = Shader.PropertyToID("_RimEnabled");
    private static readonly int RimAttenID = Shader.PropertyToID("_RimAttenuation");
    private static readonly int OutlineTypeID = Shader.PropertyToID("_OutlineType");
    private static readonly int OutlineColorID = Shader.PropertyToID("_OutlineColor");
    private static readonly int HitEnableID = Shader.PropertyToID("_HitEnabled");
    private static readonly int HitBlendID = Shader.PropertyToID("_HitBlend");

    private const int OUTLINE_SIMPLE = 1;

    [Header("References")]
    [SerializeField] private Renderer[] _renderers;

    [Header("Fade (Death)")]
    [SerializeField] private float fadeDuration = 3f;

    [Header("Buff Rim")]
    [SerializeField] private float rimFadeDuration = 0.3f;

    [Header("Hit")]
    [SerializeField] private float hitFadeDuration = 0.3f;

    [Header("Outline Colors")]
    [SerializeField] private Color outlineOffColor = Color.black;
    [SerializeField] private Color outlineOnColor = Color.red;

    private MaterialPropertyBlock _mpb;
    private Tween _fadeTween;
    private Tween _rimTween;
    private Tween _hitTween;

    public void Init()
    {
        _renderers = GetComponentsInChildren<Renderer>(false);
        _mpb = new MaterialPropertyBlock();
        InitializeOutline();
        ResetAll();
    }

    #region Fade (Death)

    [Button("Do Fade")]
    public void PlayDeathFade()
    {
        _fadeTween?.Kill();

        _fadeTween = DOTween.To(
            () => 0f,
            SetFade,
            1f,
            fadeDuration
        );
    }

    private void SetFade(float value)
    {
        foreach (var renderer in _renderers)
        {
            renderer.GetPropertyBlock(_mpb);
            _mpb.SetFloat(FadeID, value);
            renderer.SetPropertyBlock(_mpb);
        }
    }

    #endregion

    #region Buff Rim

    [Button("Enable Buff Rim")]
    public void EnableBuffRim()
    {
        _rimTween?.Kill();
        SetRim(true, 0f);

        _rimTween = DOTween.To(
            () => 0f,
            value => SetRim(true, value),
            1f,
            rimFadeDuration
        );
    }

    [Button("Disable Buff Rim")]
    public void DisableBuffRim()
    {
        _rimTween?.Kill();

        _rimTween = DOTween.To(
            () => 1f,
            value => SetRim(true, value),
            0f,
            rimFadeDuration
        ).OnComplete(() =>
        {
            SetRim(false, 0f);
        });
    }

    private void SetRim(bool enabled, float attenuation)
    {
        foreach (var renderer in _renderers)
        {
            renderer.GetPropertyBlock(_mpb);
            _mpb.SetFloat(RimEnableID, enabled ? 1f : 0f);
            _mpb.SetFloat(RimAttenID, attenuation);
            renderer.SetPropertyBlock(_mpb);
        }
    }

    #endregion

    #region Super Armor Outline (Color Only)

    private void InitializeOutline()
    {
        foreach (var renderer in _renderers)
        {
            renderer.GetPropertyBlock(_mpb);
            _mpb.SetInt(OutlineTypeID, OUTLINE_SIMPLE);     // Always Simple
            _mpb.SetColor(OutlineColorID, outlineOffColor);
            renderer.SetPropertyBlock(_mpb);
        }
    }

    [Button("Enable Super Armor")]
    public void EnableSuperArmorOutline()
    {
        SetOutlineColor(outlineOnColor);
    }

    [Button("Disable Super Armor")]
    public void DisableSuperArmorOutline()
    {
        SetOutlineColor(outlineOffColor);
    }

    private void SetOutlineColor(Color color)
    {
        foreach (var renderer in _renderers)
        {
            renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(OutlineColorID, color);
            renderer.SetPropertyBlock(_mpb);
        }
    }

    #endregion

    #region Hit

    [Button("Play Hit")]
    public void PlayHit()
    {
        _hitTween?.Kill();
        SetHit(true, 0.1f);

        _hitTween = DOTween.To(
            () => 0.1f,
            value => SetHit(true, value),
            0f,
            hitFadeDuration
        ).OnComplete(() =>
        {
            SetHit(false, 0f);
        });
    }

    private void SetHit(bool enabled, float blend)
    {
        foreach (var renderer in _renderers)
        {
            renderer.GetPropertyBlock(_mpb);
            _mpb.SetFloat(HitEnableID, enabled ? 1f : 0f);
            _mpb.SetFloat(HitBlendID, blend);
            renderer.SetPropertyBlock(_mpb);
        }
    }

    #endregion

    #region Reset

    [Button("Reset")]
    public void ResetAll()
    {
        _fadeTween?.Kill();
        _rimTween?.Kill();
        _hitTween?.Kill();

        SetFade(0f);
        SetRim(false, 0f);
        SetOutlineColor(outlineOffColor);
        SetHit(false, 0f);

        Debug.Log("[ShaderFeedback] Reset");
    }

    #endregion
}
