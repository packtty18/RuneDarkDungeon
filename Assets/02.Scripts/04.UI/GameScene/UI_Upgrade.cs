using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    [SerializeField] private UpgradeDataSO _upgradeDB;
    private UpgradeManager _upgradeManager;
    private IngredientManager _ingredientManager;

    private void Awake()
    {
        _ingredientManager = new(_upgradeDB);
        _upgradeManager = new(_ingredientManager);
        _ingredientManager.Ingredients.Subscribe(RegisterSlot);
    }

    public void Upgrade()
    {
        _upgradeManager.Upgrade();
    }
    
    private void RegisterSlot(ItemData itemData)
    {
        ItemSO itemInfo = _upgradeManager.GetItemInfo(itemData);
        Debug.Log($"강화 슬롯에 등록 [{itemInfo.Name}] : {itemInfo.Tooltip}");
    }
}
