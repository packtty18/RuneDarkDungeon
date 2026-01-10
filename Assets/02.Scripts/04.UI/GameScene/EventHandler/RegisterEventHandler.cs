using UnityEngine;

public class RegisterEventHandler : MonoBehaviour, ISlotEventHandler
{
    [SerializeField] private UpgradeManager _upgradeManager;
    
    public void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _upgradeManager.Register(slot.Item);
    }
    
    public void OnHoverSlot(UI_Slot slot) { }
    public void OnEnter() { }
    public void OnExit() { }
}
