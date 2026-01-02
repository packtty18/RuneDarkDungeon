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
        File.Load(_data);
    }
    
    private void OnApplicationQuit()
    {
        File.Save(_data);
    }
}
