using UnityEngine;

public class UpgradePresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_UpgradeInfo _upgradeInfoUI;
    [SerializeField] private UI_SlotContainer _inventoryUI;

    [Header("로직 연결")]
    [SerializeField] private SlotEventHandler _upgradeEventHandler;

    private IReadOnlyInventory _upgradeInventory;
    private IForge _forge;
    
    public void Initialize(IReadOnlyInventory upgradeInventory, IForge forge)
    {
        _upgradeInventory = upgradeInventory;
        _upgradeInventory.Subscribe(RefreshView);
        
        _forge = forge;
        _forge.Subscribe(RefreshInfo);
        
        _upgradeEventHandler.SetMode(new UnregisterEventHandler(forge));
    }

    private void OnDestroy()
    {
        _upgradeInventory.Unsubscribe(RefreshView);
        _forge.Unsubscribe(RefreshInfo);
    }

    public void Show()
    {
        _upgradeUI.Show();
        RefreshView();
        RefreshInfo();
    }

    public void Hide()
    {
        _upgradeUI.Hide();
        _inventoryUI.RefreshFilter();
    }

    private void RefreshView()
    {
        _upgradeUI.Refresh(_upgradeInventory.Items);
    }

    private void RefreshInfo()
    {
        _upgradeUI.SetSlotCount(_forge.UpgradeData.Count);
        _upgradeInfoUI.Refresh(_forge.UpgradeData);
        _inventoryUI.RefreshFilter(_upgradeInventory);
    }

    public void Upgrade()
    {
        _forge.Upgrade();
    }
}
