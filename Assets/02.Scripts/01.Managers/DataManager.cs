using System.Collections.Generic;
using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>
{
    private GameData _data = new();
    
    // Todo: 읽기 한정자 인터페이스 추가
    private GoldData Gold => _data.Gold;
    private List<RuneData> Runes => _data.Runes;

    protected override void OnInit()
    {
        FileIO.Load(_data);
    }
    
    #region Gold
    public void AddGold(int amount)
    {
        Gold.Add(amount);
    } 

    public bool UseGold(int amount)
    {
        return Gold.TryConsume(amount);
    }
    #endregion
    
    #region Rune
    public void AddRune(RuneData rune)
    {
        Runes.Add(rune);
    }
    #endregion
    
    private void OnApplicationQuit()
    {
        FileIO.Save(_data);
    }
}
