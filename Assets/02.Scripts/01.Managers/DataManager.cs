using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>
{
    private int _gold;
    private List<RuneData> _runes;

    public void InitData(GameData data)
    {
        _gold = data.Gold;
        _runes = data.Runes;
    }
}
