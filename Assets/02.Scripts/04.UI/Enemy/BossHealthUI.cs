using UnityEngine;

public class BossHealthUI : EnemyHealthUI
{
    protected override void OnInit()
    {
        _healthStat = _stat.GetValue(EEnemyConsumableFloat.Health);
        if (_healthStat == null)
        {
            Debug.LogWarning("[EnemyHealthUI] Health stat is null!");
            return;
        }
        base.OnInit();
        float ratio = _healthStat.Current / _healthStat.Max;

        SetHealthInstant(ratio);

        _healthStat.Unsubscribe(OnHealthChanged); // 중복 방지
        _healthStat.Subscribe(OnHealthChanged);

        Show();
        _isInitialized = true;
    }
}
