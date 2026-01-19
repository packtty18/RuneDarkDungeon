using System.Xml.Serialization;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ComboPresenter : MonoBehaviour
{
    [SerializeField]
    private PlayerContext _playerContext;

    [SerializeField]
    private TextMeshProUGUI _comboText;
    [SerializeField]
    private UI_BasicAnimation _combo;
    [SerializeField]
    private UI_BasicAnimation _finisher;
    [SerializeField]
    private UI_BasicAnimation _charging;
    [SerializeField]
    private UI_BasicAnimation _chargingFinisher;

    private PlayerAttack _playerAttack;

    private void Awake()
    {
        if (_playerContext.Attack != null)
            Bind();

        _playerContext.Subscribe(Bind);
    }
    private void Bind()
    {
        _playerAttack = _playerContext.Attack;

        _playerAttack.OnComboChange += ComboUpdate;
        _playerAttack.OnComboCharging += OnCharging;
    }
    void Start()
    {
        HideAll();
    }

    void ComboUpdate (float currentCombo, float maxCombo)
    {
        if (currentCombo == 0)
        {
            FadeOutAll();
        }
        else if (currentCombo == maxCombo)
        {
            _charging.Stop();
            _charging.Hide();
            _finisher.PunchFadeIn();
        }
        else
        {
            _comboText.text = $"{currentCombo}";
            _combo.Show();
            _combo.ScalePunch();
        }
    }

    void OnCharging(bool _isCharged)
    {
        if (!_isCharged)
        {
            _combo.Hide();
            _charging.Show();
            _charging.ShakeLoop(50);
        }
        else
        {
            HideAll();
            _charging.StopAndReset();
            _chargingFinisher.PunchFadeIn();
        }
    }

    private void HideAll()
    {
        _combo.Hide();
        _finisher.Hide();
        _charging.Hide();
        _chargingFinisher.Hide();
    }

    private void FadeOutAll()
    {
        _combo.FadeOut();
        _finisher.FadeOut();
        _chargingFinisher.FadeOut();
    }

    private void OnDestroy()
    {
        _playerContext?.Unsubscribe(Bind);
        if (_playerAttack != null)
        {
            _playerAttack.OnComboChange -= ComboUpdate;
            _playerAttack.OnComboCharging -= OnCharging;
        }
    }
}
