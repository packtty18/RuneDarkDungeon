using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryFilter : UI_Base
{
    private IReadOnlyList<UI_Slot> _slots;
    
    private UpgradeManager _upgradeManager;
    private IReadOnlyInventory _upgradeInventory;

    public void Initialize(UpgradeManager upgradeManager, IReadOnlyInventory upgradeInventory, IReadOnlyList<UI_Slot> inventorySlots)
    {
        _upgradeManager = upgradeManager;
        _upgradeInventory = upgradeInventory;
        
        _slots = inventorySlots;
        
        upgradeManager.Subscribe(Refresh);
    }

    private void OnDestroy()
    {
        _upgradeManager.Unsubscribe(Refresh);
    }

    private void Refresh()
    {
        RefreshSlotState();
    }

    public override void Show()
    {
        RefreshSlotState();
    }

    public override void Hide()
    {
        ResetSlotState();
    }

    // 강화 모드를 끌 때 인벤토리 슬롯 참조, 어차피 다 끄므로 순서 상관없음
    private void ResetSlotState()
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty)
            {
                slot.SetActive(false);
                continue;
            }
            slot.SetInteractable(true);
        }
    }
    
    // 얘 차라리 슬롯 컨테이너를 구독해서 슬롯도 직접 받아오고 슬롯이 refresh할 때마다 필터링해주는건 어떤가용..
    // 왜냐하면 아무래도 강화 정보가 바뀔 때 갱신하는 느낌보다는 인벤토리가 갱신될 때 바뀌는 느낌이자나요
    private void RefreshSlotState()
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty)
            {
                slot.SetActive(false);
                continue;
            }
            bool isOn = _upgradeInventory.CanAdd(slot.Item);
            slot.SetInteractable(isOn);
            slot.SetActive(true);
        }
    }
}
