using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy/Spawner/PhaseData")]
public class EnemySpawnerDataSO : ScriptableObject
{
    public List<EnemySpawnData> Datas = new();
}

[Serializable]
public struct EnemySpawnData
{
    public EEnemyType Type;
    public int Count;

}