using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartData", menuName = "Data/StartData")]
public class StartDataSO : ScriptableObject
{
    [SerializeField] private int _gold;
    [SerializeField] private List<ItemData> _inventory;
    [SerializeField] private SerializableDictionary<ESkillSlot, ItemData> _equipment;
    
    public int Gold => _gold;
    public List<ItemData> Inventory => _inventory;
    public SerializableDictionary<ESkillSlot, ItemData> Equipment => _equipment;
}
