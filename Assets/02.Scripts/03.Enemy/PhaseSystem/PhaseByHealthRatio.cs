using UnityEngine;

public class PhaseByHealthRatio : IPhaseTransition
{
    [SerializeField, Range(0f, 1f)]
    private float _triggerHealthRatio;

    public PhaseByHealthRatio(float triggerRatio)
    {
        _triggerHealthRatio = triggerRatio;
    }

    public bool ShouldTransition(EnemyController controller)
    {
        float currentRatio = controller.Stat.GetValue(EEnemyConsumableFloat.Health).GetRatio();
        return currentRatio <= _triggerHealthRatio;
    }

    public string GetDescription()
    {
        return $"체력 {_triggerHealthRatio * 100}% 이하";
    }
}
