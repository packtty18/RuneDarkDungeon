using DG.Tweening;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class HpBarPresenter : MonoBehaviour
{
    [SerializeField]
    private PlayerContext _playerContext;

    private PlayerStats _playerStats;
    private UnityEngine.UI.Slider _slider;

    private void Awake()
    {
        if (_playerContext.Stats != null)
            Bind();

        _playerContext.Subscribe(Bind);
        _slider = GetComponent<Slider>();
    }

    private void Bind()
    {
        _playerStats = _playerContext.Stats;
        _playerStats.Health.Subscribe(OnHpUpdate);
        _slider.value = _playerStats.Health.GetRatio();
    }

    private void OnHpUpdate(float value)
    {
        var ratio = _playerStats.Health.GetRatio();
        _slider.DOKill();
        _slider.DOValue(ratio, 0.5f);
    }

    private void OnDestroy()
    {
        _playerContext?.Unsubscribe(Bind);
        _playerStats?.Health.Unsubscribe(OnHpUpdate);
    }
}
