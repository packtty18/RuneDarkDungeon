using UnityEngine;

public class EquipmentPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_EquipmentView _equipmentUI;
    [SerializeField] private ModePresenter _modePresenter;
    
    [Header("로직 연결")]
    [SerializeField] private InventoryManager _inventoryManager;
    
    private IEquipment _equipment;
    private IInventory _inventory;

    public void Initialize(IEquipment equipment, IInventory inventory)
    {
        _equipment = equipment;
        _inventory = inventory;
        
        _equipment.Subscribe(RefreshEquipment);
        _equipmentUI.OnSlotDoubleClicked += HandleSlotDoubleClicked;
        _equipmentUI.OnSlotClicked += HandleSlotClicked;
        _equipmentUI.OnSlotHovered += HandleSlotHovered;
        
        RefreshEquipment();
        
        _modePresenter.Subscribe(HandleModeChanged);
    }

    private void OnDestroy()
    {
        _equipment.Unsubscribe(RefreshEquipment);
        _equipmentUI.OnSlotDoubleClicked -= HandleSlotDoubleClicked;
        _equipmentUI.OnSlotClicked -= HandleSlotClicked;
        _equipmentUI.OnSlotHovered -= HandleSlotHovered;
        _modePresenter.Unsubscribe(HandleModeChanged);
    }
    
    private void RefreshEquipment()
    {
        _equipmentUI.Refresh(_equipment);
    }

    private void HandleSlotDoubleClicked(ESkillSlot slot)
    {
        ItemData item = _equipment.UnEquip(slot);

        if (item == null) return;
        _inventory.Add(item);
    }

    private void HandleSlotClicked(ESkillSlot slot)
    {
        var item = _inventoryManager.GetSelectedItem();
        if (item == null) return;
        
        _inventory.Remove(item);
        ItemData oldItem = _equipment.Equip(slot, item);

        if (oldItem == null) return;
        _inventory.Add(oldItem);
    }

    private void HandleSlotHovered(UI_Slot slot)
    {
        _inventoryManager.ShowTooltip(slot);
    }

    private void HandleModeChanged(EInventoryMode mode)
    {
        if (mode == EInventoryMode.Equipment)
        {
            _equipmentUI.Show();
        }
        else
        {
            _equipmentUI.Hide();
        }
    }
}
