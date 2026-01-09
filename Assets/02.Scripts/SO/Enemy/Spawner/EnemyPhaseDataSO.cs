using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy/Spawner/PhaseData")]
public class EnemyPhaseDataSO : ScriptableObject
{
    public List<PhaseData> Datas = new();
}

[Serializable]
public class PhaseData
{
    public EEnemyType Type;
    public int Count;

    [Tooltip("랜덤으로 스폰 혹은 순서대로 균일하게 스폰")]
    public bool IsRandomSpawn;
}