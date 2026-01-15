using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ItemDropper : PoolSpawner
{
    [Header("Drop Table")]
    [SerializeField] private ItemDropTableSO _dropTable;

    [Header("Spawn")]
    [SerializeField] private float _spawnRadius = 0.5f;

    [Button]
    public void Drop()
    {
        if (_dropTable == null)
        {
            Debug.LogWarning("[ItemDropper] 드롭 테이블을 설정하세요.");
            return;
        }

        List<EPoolType> results = _dropTable.GetDropResult();

        foreach (var poolType in results)
        {
            SpawnItem(poolType);
        }

        Debug.LogWarning("[ItemDropper] 드롭 완료");
    }

    private void SpawnItem(EPoolType poolType)
    {
        Vector3 offset = Random.insideUnitSphere * _spawnRadius;
        offset.y = 0f;

        GameObject obj = GetFromPool(poolType);
        if(!obj.TryGetComponent(out ItemBase item))
        {
            Util.ObjectDestroy(obj);
            return;
        }
        item.transform.position = transform.position + offset;
    }
}
