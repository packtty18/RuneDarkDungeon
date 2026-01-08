using UnityEngine;

public interface ISlotEventHandler
{
    void OnClickSlot(UI_Slot slot);
    void OnHoverSlot(UI_Slot slot);
    void OnEnter();
    void OnExit();
    
    bool IsInteractable(UI_Slot slot, ItemData item);
}
