using System;
using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>
{
    private GameData _data = new();
    
    private GoldData _gold;
    private Inventory _inventory;

    public IReadOnlyValue<int> Gold => _gold;
    public IReadOnlyInventory Inventory => _inventory;
    
    protected override void OnInit()
    {
        FileIO.Load(_data);
        _gold = _data.Gold;
        _inventory = _data.Inventory;
    }
    
    #region Gold
    public void AddGold(int amount)
    {
        _gold.Add(amount);
    } 

    public bool UseGold(int amount)
    {
        return _gold.TryConsume(amount);
    }

    public void SubscribeGold(Action<int> action)
    {
        _gold.Subscribe(action);
    }

    public void UnsubscribeGold(Action<int> action)
    {
        _gold.Unsubscribe(action);
    }
    #endregion
    
    #region Rune
    public void AddRune(RuneData rune)
    {
        _inventory.Add(rune);
    }
    
    public void SubscribeRune(Action<RuneData> action)
    {
        _inventory.Subscribe(action);
    }

    public void UnsubscribeRune(Action<RuneData> action)
    {
        _inventory.Unsubscribe(action);
    }
    #endregion
    
    private void AddData()
    {
        AddGold(1000);
        AddRune(new RuneData(0));
        AddRune(new RuneData(1, 5));
    }
    
    private void OnApplicationQuit()
    {
        AddData();
        FileIO.Save(_data);
    }
}
