using System;
using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>
{
    private GameData _data = new();
    
    // Todo: 읽기 한정자 인터페이스 추가
    private GoldData _gold;
    private Inventory _inventory;
    
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

    public void UnsubscribedGold(Action<int> action)
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
    
    private void OnApplicationQuit()
    {
        FileIO.Save(_data);
    }
}
