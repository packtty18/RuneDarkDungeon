using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HPUIUpdate : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider _slider;
    [SerializeField] private PlayerStats _playerStats;

    void Start()
    {
        _playerStats.Health.Subscribe(OnHpUpdate);
    }

    private void OnHpUpdate(float value)
    {
        var ratio = _playerStats.Health.GetRatio();
        _slider.DOKill();
        _slider.DOValue(ratio, 0.5f);
    }

    private void OnDestroy()
    {
        _playerStats.Health.Unsubscribe(OnHpUpdate);
    }
}
