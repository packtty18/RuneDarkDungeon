using UnityEngine;

public class UpgradePresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_UpgradeInfo _upgradeInfoUI;
    [SerializeField] private UI_SlotContainer _inventoryUI;

    private IForge _forge;
    private IInventory _inventory;
    private ICurrency _currency;
    
    public void Initialize(IForge forge, IInventory inventory, ICurrency currency)
    {
        _forge = forge;
        _inventory = inventory;
        _currency = currency;
        
        _upgradeUI.OnSlotClicked += HandleSlotClicked;
        _forge.Subscribe(Refresh);
    }

    private void OnDestroy()
    {
        _upgradeUI.OnSlotClicked -= HandleSlotClicked;
        _forge.Unsubscribe(Refresh);
    }

    private void Refresh()
    {
        _upgradeUI.Refresh(_forge.Items);
        _upgradeUI.SetSlotCount(_forge.UpgradeData.Count);
        _upgradeInfoUI.Refresh(_forge.UpgradeData);
        _inventoryUI.RefreshFilter(_forge);
    }

    private void HandleSlotClicked(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _inventory.Add(slot.Item);
        _forge.Unregister(slot.Item);
        _forge.Notify();
    }

    public void HandleModeChanged(EInventoryMode mode)
    {
        if (mode == EInventoryMode.Upgrade)
        {
            _upgradeUI.Show();
            Refresh();
        }
        else
        {
            _upgradeUI.Hide();
            _inventoryUI.RefreshFilter();
        }
    }
        
    public void Upgrade()
    {
        if (!_forge.Upgrade(_currency, out var item)) return;
        _inventory.Add(item);
        _forge.Notify();
    }
}
