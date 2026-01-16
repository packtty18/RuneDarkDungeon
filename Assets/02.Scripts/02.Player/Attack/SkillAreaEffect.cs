using System;
using UnityEngine;

public class SkillAreaEffect : MonoBehaviour
{
    #region Serialized Fields

    [Header("Component Settings")]
    [SerializeField] private bool isParticleSystem;
    [SerializeField] private Material inputMaterial;

    [Header("Fade Out Settings")]
    [SerializeField] private float timeToReduce = 1f;
    [SerializeField] private float reduceFactor = 1f;

    [Header("Fade In Settings")]
    [SerializeField] private float _upFactor = 0f;

    [SerializeField] private EffectPlayer player;

    #endregion

    #region Private Fields

    private Material _effectMaterial;
    private MeshRenderer _meshRenderer;
    private ParticleSystemRenderer _particleRenderer;
    private HitBox _hitBox;

    private float _elapsedTime;
    private float _currentReduceFactor;
    private float _fadeOutValue;
    private float _fadeInValue;

    private bool _isFadingIn = true;
    private bool _isActive = false;

    #endregion

    #region Unity Lifecycle

    void Awake()
    {
        InitializeComponents();
        InitializeMaterial();
        ResetValues();
    }

    private void Start()
    {
        RegisterEvents();
        _effectMaterial.SetFloat("_MaskCutOut", 0);
    }

    void LateUpdate()
    {
        if (!_isActive) return;

        if (_upFactor > 0 && _isFadingIn)
        {
            UpdateFadeIn();
        }
        else
        {
            UpdateFadeOut();
        }
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #endregion

    #region Initialization

    private void InitializeComponents()
    {
        player = GetComponentInParent<EffectPlayer>();
        _hitBox = GetComponent<HitBox>();

        if (player == null)
        {
            Debug.LogError($"[SkillAreaEffect] EffectPlayer not found in parent of {gameObject.name}!");
        }

        if (_hitBox == null)
        {
            Debug.LogWarning($"[SkillAreaEffect] HitBox component not found on {gameObject.name}");
        }
    }

    private void InitializeMaterial()
    {
        if (isParticleSystem)
        {
            _particleRenderer = GetComponent<ParticleSystemRenderer>();
            if (_particleRenderer != null && inputMaterial != null)
            {
                _particleRenderer.material = inputMaterial;
                _effectMaterial = _particleRenderer.material;
            }
        }
        else
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshRenderer != null && inputMaterial != null)
            {
                _meshRenderer.material = inputMaterial;
                _effectMaterial = _meshRenderer.material;
            }
        }

        if (_effectMaterial == null)
        {
            Debug.LogError($"[SkillAreaEffect] Material initialization failed on {gameObject.name}!");
        }
    }

    private void ResetValues()
    {
        _currentReduceFactor = 0f;
        _fadeOutValue = 1f;
        _fadeInValue = 0f;
        _elapsedTime = 0f;
        _isFadingIn = true;
        _isActive = false;
    }

    #endregion

    #region Event Handling

    private void RegisterEvents()
    {
        if (player != null)
        {
            player.OnPlay += OnSkillActivated;
            player.OnAttack += OnSkillDamageTriggered;
        }
    }

    private void UnregisterEvents()
    {
        if (player != null)
        {
            player.OnPlay -= OnSkillActivated;
            player.OnAttack -= OnSkillDamageTriggered;
        }
    }

    private void OnSkillActivated()
    {
        ResetValues();
        _isActive = true;

        float initialCutOut = (_upFactor > 0) ? 0f : 1f;
        _effectMaterial.SetFloat("_MaskCutOut", initialCutOut);
    }

    private void OnSkillDamageTriggered(float damage)
    {
        if (_hitBox != null)
        {
            _hitBox.Activate(damage);
        }
    }

    #endregion

    #region Fade Animation

    private void UpdateFadeIn()
    {
        _fadeInValue += _upFactor * Time.deltaTime;
        _fadeInValue = Mathf.Clamp01(_fadeInValue);
        _effectMaterial.SetFloat("_MaskCutOut", _fadeInValue);

        if (_fadeInValue >= 1f)
        {
            _isFadingIn = false;
            DeactivateHitBox();
        }
    }

    private void UpdateFadeOut()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > timeToReduce)
        {
            _fadeOutValue -= _currentReduceFactor;
            _currentReduceFactor = Mathf.Lerp(_currentReduceFactor, reduceFactor, Time.deltaTime / 50f);
            _fadeOutValue = Mathf.Clamp01(_fadeOutValue);

            if (_fadeOutValue <= 0f)
            {
                _isActive = false;
                DeactivateHitBox();
            }

            _effectMaterial.SetFloat("_MaskCutOut", _fadeOutValue);
        }
    }

    #endregion

    #region HitBox Control

    private void DeactivateHitBox()
    {
        if (_hitBox != null)
        {
            _hitBox.Deactivate();
        }
    }

    #endregion
}