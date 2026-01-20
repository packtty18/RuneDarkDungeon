using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingPresenter : MonoBehaviour
{
    private const int MaxDisplayValue = 10;
    
    [Header("BGM 설정")]
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private TextMeshProUGUI _bgmVolumeTextUI;
    [SerializeField] private GameObject _bgmIconOn;
    [SerializeField] private GameObject _bgmIconOff;
    
    [Header("SFX 설정")]
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI _sfxVolumeTextUI;
    [SerializeField] private GameObject _sfxIconOn;
    [SerializeField] private GameObject _sfxIconOff;

    [Header("색상 설정")]
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _zeroColor;
    
    private void Start()
    {
        _bgmVolumeSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        _sfxVolumeSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        
        InitializeUI();
    }

    private void InitializeUI()
    {
        float currentBgm = SoundManager.Instance.GlobalBgmVolume;
        float currentSfx = SoundManager.Instance.GlobalSfxVolume;
        
        _bgmVolumeSlider.value = currentBgm;
        _sfxVolumeSlider.value = currentSfx;
        
        UpdateBgmUI(currentBgm);
        UpdateSfxUI(currentSfx);
    }

    private void OnBgmSliderChanged(float value)
    {
        SoundManager.Instance.SetBgmVolume(value);
        UpdateBgmUI(value);
    }

    private void OnSfxSliderChanged(float value)
    {
        SoundManager.Instance.SetSfxVolume(value);
        UpdateSfxUI(value);
    }

    private void UpdateBgmUI(float inputValue)
    {
        int value = Mathf.CeilToInt(inputValue * MaxDisplayValue);
        _bgmVolumeTextUI.SetText("{0}", value);

        bool isMuted = value == 0;
        _bgmVolumeTextUI.color = isMuted ?  _zeroColor : _normalColor;
        _bgmIconOn.SetActive(!isMuted);
        _bgmIconOff.SetActive(isMuted);
    }

    private void UpdateSfxUI(float inputValue)
    {
        int value = Mathf.CeilToInt(inputValue * MaxDisplayValue);
        _sfxVolumeTextUI.SetText("{0}", value);
        
        bool isMuted = value == 0;
        _sfxVolumeTextUI.color = isMuted ? _zeroColor : _normalColor;
        _sfxIconOn.SetActive(!isMuted);
        _sfxIconOff.SetActive(isMuted);
    }
}
