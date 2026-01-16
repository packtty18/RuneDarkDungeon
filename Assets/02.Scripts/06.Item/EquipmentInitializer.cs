using UnityEngine;

public class EquipmentInitializer : MonoBehaviour
{
    [Header("연결 대상 플레이어")]
    [SerializeField] private PlayerSkillCaster _playerSkillCaster;

    private void Awake()
    {
        IDataHolder dataHolder = DataManager.Instance;
        _playerSkillCaster.Initialize(dataHolder.Equipment);
    }
}
