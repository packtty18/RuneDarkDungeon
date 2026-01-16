using System.Xml.Serialization;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ComboUIUpdate : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _comboText;
    [SerializeField]
    private UIBasicAnimation _combo;
    [SerializeField]
    private UIBasicAnimation _finisher;
    [SerializeField]
    private UIBasicAnimation _charging;
    [SerializeField]
    private UIBasicAnimation _chargingFinisher;

    [SerializeField]
    private PlayerAttack _playerAttack;

    void Start()
    {
        HideAll();
        _playerAttack.OnComboChange += ComboUpdate;
        _playerAttack.OnComboCharging += OnCharging;  
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
        if (_playerAttack != null)
        {
            _playerAttack.OnComboChange -= ComboUpdate;
            _playerAttack.OnComboCharging -= OnCharging;
        }
    }
}
