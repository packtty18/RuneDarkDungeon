using UnityEngine;

public class RewardManager : MonoBehaviour
{
    private IInventory _inventory;
    private ICurrency _currency;
    private ItemFactory _itemFactory;
    
    [SerializeField] private ItemDatabaseSO _itemDB;

    private void Awake()
    {
        _itemFactory = new(_itemDB);
    }

    private void Start()
    {
        Initialize(DataManager.Instance);
    }
    
    public void Initialize(IDataHolder data)
    {
        _inventory = data.Inventory;
        _currency = data.GoldData;
        
        GameEvents.OnCoinCollected += GetGold;
        GameEvents.OnRuneCollected += GetRandomRune;
    }

    private void OnDestroy()
    {
        GameEvents.OnCoinCollected -= GetGold;
        GameEvents.OnRuneCollected -= GetRandomRune;
    }

    private void GetGold(int amount)
    {
        _currency.Add(amount);
    }

    private void GetRandomRune(EItemGrade grade)
    {
        var newItem = _itemFactory.CreateRandomItem(grade);
        _inventory.Add(newItem);
    }
}
