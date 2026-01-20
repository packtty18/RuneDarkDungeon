using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    private IInventory _inventory;
    private ICurrency _currency;
    private ItemFactory _itemFactory;
    
    [SerializeField] private ItemDatabaseSO _itemDB;

    private readonly GoldData _rewardGold = new();
    private readonly List<ItemData> _rewardItems = new();
    
    public IReadOnlyCurrency RewardGold => _rewardGold;
    public IReadOnlyList<ItemData> RewardItems => _rewardItems;
    
    private void Awake()
    {
        _itemFactory = new(_itemDB);
    }

    private void Start()
    {
        if (DataManager.Instance == null) return;
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
        _rewardGold.Add(amount);
    }

    private void GetRandomRune(EItemGrade grade)
    {
        var newItem = _itemFactory.CreateRandomItem(grade);
        _inventory.Add(newItem);
        _rewardItems.Add(newItem);
    }
}
