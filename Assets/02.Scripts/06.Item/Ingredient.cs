using UnityEngine;

public class Ingredient
{
    private readonly Inventory _ingredients = new();
    public IReadOnlyInventory Ingredients => _ingredients;

    private UpgradeDataSO _upgradeDB;
    
    private RuneData _targetType;
    private UpgradeData _upgradeData;
    
    public int Cost => _upgradeData.Cost;
    private int Count => _upgradeData.Count;
    public float Rate => _upgradeData.Rate;

    public Ingredient(UpgradeDataSO upgradeDB)
    {
        _upgradeDB = upgradeDB;
    }
    
    public bool CanRegister(RuneData rune)
    {
        if (_targetType == null) return true;
        return rune.TypeEquals(_targetType) && _ingredients.Count < Count;
    }

    public void Register(RuneData rune)
    {
        if (_targetType == null)
        {
            RegisterTargetType(rune);
        }
        _ingredients.Add(rune);
    }

    public bool CanUpgrade()
    {
        if (_targetType == null || _ingredients.Count < Count) return false;
        return true;
    }
    
    public RuneData GetUpgradeResult()
    {
        RuneData newRune = new(_targetType.ID, _targetType.Grade + 1);
        return newRune;
    }

    public RuneData Unregister(RuneData rune)
    {
        _ingredients.Remove(rune);

        if (_ingredients.Count == 0)
        {
            _targetType = null;
        }
        
        return rune;
    }

    public void Clear()
    {
        _ingredients.Clear();
        _targetType = null;
    }

    private void RegisterTargetType(RuneData rune)
    {
        var info = _upgradeDB.GetGradeInfo(rune.Grade);
        if (info == null) return;
        
        _targetType = rune;
        _upgradeData = info.Value;
    }
}
