using UnityEngine;

public class EquipEventHandler : ISlotEventHandler
{
    private readonly EquipmentManager _equipmentManager;
    private readonly ItemDatabaseSO _itemDB;

    public EquipEventHandler(EquipmentManager manager, ItemDatabaseSO itemDB)
    {
        _equipmentManager = manager;
        _itemDB = itemDB;
    }

    public void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;

        Debug.Log($"[장착 시도] {slot.Item.ID}");
        
        _equipmentManager.EquipItem(ESkillSlot.Q, slot.Item);
    }
    
    public void OnHoverSlot(UI_Slot slot) { }
    public void OnEnter() { }
    public void OnExit() { }
}
