using System;
using UnityEngine;
using UnityEngine.UI;

public class HPUIUpdate : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerStats _playerStats;

    void Start()
    {
        _playerStats.Health.Subscribe(OnHpUpdate);
    }

    private void OnHpUpdate(float value)
    {
        //_slider.value = _playerStats.Health.GetRatio();
        _slider.value = value/ 1000f;
    }

    private void OnDestroy()
    {
        _playerStats.Health.Unsubscribe(OnHpUpdate);
    }
}
