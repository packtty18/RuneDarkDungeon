using System.Collections.Generic;
using System;
using UnityEngine;

// 테스트용 룬 데이터
[System.Serializable]
public class ItemData
{
    [SerializeField] private int _id;
    [SerializeField] private int _level;
    
    public int ID => _id;
    public int Level => _level;

    public ItemData(int id, int level = 1)
    {
        _id = id; 
        _level = level;
    }
    
    public void LevelUp() => ++_level;
    
    public override string ToString() => $"[ID:{_id}] Lv.{_level})";
}
