using UnityEngine;

public class UpgradePresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_UpgradeInfo _upgradeInfoUI;
    [SerializeField] private UI_SlotContainer _inventoryUI;

    [Header("로직 연결")]
    [SerializeField] private UpgradeManager _upgradeManager;
    
    [SerializeField] private SlotEventHandler _upgradeEventHandler;
    [SerializeField] private UnregisterEventHandler _unregisterEventHandler;

    private IInventory _upgradeInventory;

    public void Initialize(IInventory upgradeInventory)
    {
        _upgradeInventory = upgradeInventory;
        _upgradeInventory.Subscribe(RefreshView);
        _upgradeManager.Subscribe(RefreshInfo);
        
        _upgradeEventHandler.SetMode(_unregisterEventHandler);
    }

    private void OnDestroy()
    {
        _upgradeInventory.Unsubscribe(RefreshView);
        _upgradeManager.Unsubscribe(RefreshInfo);
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
        _upgradeUI.SetSlotCount(_upgradeManager.UpgradeData.Count);
        _upgradeInfoUI.Refresh(_upgradeManager.UpgradeData);
        _inventoryUI.RefreshFilter(_upgradeInventory);
    }
}
