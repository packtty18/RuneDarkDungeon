using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Item/Item Drop Table")]
public class ItemDropTableSO : ScriptableObject
{
    [SerializeField] private List<ItemDropEntry> entries = new();

    public List<EPoolType> GetDropResult()
    {
        List<EPoolType> result = new();

        if (entries.Count == 0 )
        {
            return result;
        }

        //확정보상이라면 무조건 리스트에 추가
        foreach (var entry in entries)
        {
            if (!entry.IsGuaranteed)
            {
                continue;
            }

            for (int i = 0; i < entry.Count; i++)
            {
                result.Add(entry.PoolType);
                
            }
            Debug.Log($"{entry.PoolType}을 {entry.Count}개 추가");
        }

        //랜덤 추가
        List<ItemDropEntry> randomPool = GetRandomPool();
        ItemDropEntry selected = GetWeightedRandom(randomPool);
        if (selected == null)
        {
            return result;
        }

        result.Add(selected.PoolType);

        Debug.Log($"{selected.PoolType}을 {selected.Count}개 추가");
        return result;
    }

    #region Utility

    private List<ItemDropEntry> GetRandomPool()
    {
        List<ItemDropEntry> pool = new();

        foreach (var entry in entries)
        {
            if (!entry.IsGuaranteed && entry.Weight > 0)
            {
                pool.Add(entry);
            }
        }

        return pool;
    }

    private ItemDropEntry GetWeightedRandom(List<ItemDropEntry> pool)
    {
        int totalWeight = 0;

        foreach (var entry in pool)
        {
            totalWeight += entry.Weight;
        }

        if (totalWeight <= 0)
        {
            return null;
        }

        int random = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var entry in pool)
        {
            cumulative += entry.Weight;
            if (random < cumulative)
            {
                return entry;
            }
        }

        return null;
    }

    #endregion
}

[Serializable]
public class ItemDropEntry
{
    public EPoolType PoolType;

    [Tooltip("체크 시 무조건 드롭")]
    public bool IsGuaranteed;
    [Min(0), HideIf(nameof(IsGuaranteed))]
    public int Weight = 1;

    [Tooltip("MinCount ~ MaxCount만큼 생성")]
    [Min(0)]
    public int Count = 1;

    
}
