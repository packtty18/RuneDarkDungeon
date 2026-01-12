using UnityEngine;

public class EquipmentPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_EquipmentView _equipmentUI;
    
    [Header("로직 연결")]
    [SerializeField] private InventoryManager _inventoryManager;
    
    private EquipmentManager _equipmentManager;
    private IEquipment _equipment;

    public void Initialize(EquipmentManager equipmentManager, IEquipment equipment)
    {
        _equipmentManager = equipmentManager;
        _equipment = equipment;
        
        _equipment.Subscribe(RefreshEquipment);
        _equipmentUI.OnSlotDoubleClicked += HandleSlotDoubleClicked;
        _equipmentUI.OnSlotClicked += HandleSlotClicked;
        
        RefreshEquipment();
    }

    private void OnDestroy()
    {
        _equipment.Unsubscribe(RefreshEquipment);
        _equipmentUI.OnSlotDoubleClicked -= HandleSlotDoubleClicked;
        _equipmentUI.OnSlotClicked -= HandleSlotClicked;
    }
    
    private void RefreshEquipment()
    {
        _equipmentUI.Refresh(_equipment);
    }

    private void HandleSlotDoubleClicked(ESkillSlot slot)
    {
        _equipmentManager.UnEquipItem(slot);
    }

    private void HandleSlotClicked(ESkillSlot slot)
    {
        var item = _inventoryManager.GetSelectedItem();
        if (item == null) return;
        
        _equipmentManager.EquipItem(slot, item);
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
