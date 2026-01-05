using UnityEngine;

public class GoldManager : GlobalSingleton<GoldManager>
{
    private GoldData _gold;
    public IReadOnlyValue<int> Gold => _gold;

    public void Initialize(GoldData goldData)
    {
        _gold = goldData;
    }

    public void AddGold(int amount)
    {
        _gold.Add(amount);
    }

    public bool UseGold(int amount)
    {
        return _gold.TryConsume(amount);
    }
}
