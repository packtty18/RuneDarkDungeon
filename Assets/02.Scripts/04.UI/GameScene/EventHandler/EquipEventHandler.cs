using UnityEngine;

public class EquipEventHandler : ISlotEventHandler
{
    private readonly EquipmentManager _equipmentManager;

    public EquipEventHandler(EquipmentManager equipmentManager)
    {
        _equipmentManager = equipmentManager;
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
