using UnityEngine;

public class UpgradePresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_UpgradeInfo _upgradeInfoUI;
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private ModePresenter _modePresenter;

    private IReadOnlyInventory _upgradeInventory;
    private IForge _forge;
    
    public void Initialize(IReadOnlyInventory upgradeInventory, IForge forge)
    {
        _upgradeInventory = upgradeInventory;
        _forge = forge;
        
        _upgradeUI.OnSlotClicked += HandleSlotClicked;
        _upgradeInventory.Subscribe(RefreshView);
        _forge.Subscribe(RefreshInfo);
        _modePresenter.Subscribe(HandleModeChanged);
    }

    private void OnDestroy()
    {
        _upgradeUI.OnSlotClicked -= HandleSlotClicked;
        _upgradeInventory.Unsubscribe(RefreshView);
        _forge.Unsubscribe(RefreshInfo);
        _modePresenter.Unsubscribe(HandleModeChanged);
    }

    private void RefreshView()
    {
        _upgradeUI.Refresh(_upgradeInventory.Items);
    }

    private void RefreshInfo()
    {
        _upgradeUI.SetSlotCount(_forge.UpgradeData.Count);
        _upgradeInfoUI.Refresh(_forge.UpgradeData);
        _inventoryUI.RefreshFilter(_forge);
    }

    private void HandleSlotClicked(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _forge.Unregister(slot.Item);
    }

    private void HandleModeChanged(EInventoryMode mode)
    {
        if (mode == EInventoryMode.Upgrade)
        {
            _upgradeUI.Show();
            RefreshView();
            RefreshInfo();
        }
        else
        {
            _upgradeUI.Hide();
            _inventoryUI.RefreshFilter();
        }
    }
        
    public void Upgrade()
    {
        _forge.Upgrade();
    }
}
