using UnityEngine;

public class EquipmentPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_EquipmentView _equipmentUI;
    
    private EquipmentManager _equipmentManager;
    private IEquipment _equipment;

    public void Initialize(EquipmentManager equipmentManager, IEquipment equipment)
    {
        _equipmentManager = equipmentManager;
        _equipment = equipment;
        
        _equipment.Subscribe(RefreshEquipment);
        _equipmentUI.OnSlotClicked += HandleSlotClicked;
        
        RefreshEquipment();
    }

    private void OnDestroy()
    {
        _equipment.Unsubscribe(RefreshEquipment);
        _equipmentUI.OnSlotClicked -= HandleSlotClicked;
    }
    
    private void RefreshEquipment()
    {
        _equipmentUI.Refresh(_equipment);
    }

    private void HandleSlotClicked(ESkillSlot slot)
    {
        _equipmentManager.UnEquipItem(slot);
    }

    public void Show()
    {
        _equipmentUI.Show();
    }

    public void Hide()
    {
        _equipmentUI.Hide();
    }
}
