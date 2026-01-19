using UnityEngine;
using Sirenix.OdinInspector;

public class TestItemMaker : MonoBehaviour
{
    [SerializeField] private ItemDatabaseSO _itemDB;
    private ItemFactory _itemFactory;
    private IInventory _inventory;
    private IEquipment _equipment;
    private ICurrency _gold;
    
    private void Start()
    {
        var data = DataManager.Instance;
        _inventory = data.Inventory;
        _equipment = data.Equipment;
        _gold = data.GoldData;
        
        _itemFactory = new ItemFactory(_itemDB);
    }
    
    [Title("Test Settings")]
    
    [BoxGroup("Item Creation"), LabelText("Item ID")]
    public int TargetId;
    
    [BoxGroup("Item Creation"), EnumToggleButtons]
    public EItemGrade TargetGrade;
    
    [BoxGroup("Item Creation"), EnumToggleButtons]
    public ESkillSlot TargetSkill;
    
    [BoxGroup("Item Creation")]
    [Button("Add To Inventory", ButtonSizes.Large), GUIColor(0.4f, 0.8f, 0.4f)]
    public void AddToInventory()
    {
        if (!VerifyAndCreateItem(out var newItem)) return;

        _inventory.Add(newItem);
        Debug.Log($"[Test] Inventory Added: ID {TargetId}, Grade {TargetGrade}");
    }
    
    [BoxGroup("Item Creation")]
    [Button("Equip Immediately", ButtonSizes.Large), GUIColor(0.4f, 0.6f, 0.8f)]
    public void EquipItem()
    {
        if (!VerifyAndCreateItem(out var newItem)) return;

        _equipment.Equip(TargetSkill, newItem);
        Debug.Log($"[Test] Equipped: ID {TargetId}, Grade {TargetGrade}");
    }
    
    [BoxGroup("Currency")]
    [LabelText("Amount")]
    public int GoldAmount = 1000;

    [BoxGroup("Currency")]
    [Button("Add Gold", ButtonSizes.Large), GUIColor(1f, 0.84f, 0f)]
    public void AddGold()
    {
        _gold.Add(GoldAmount); 
        
        Debug.Log($"[Test] Gold Added: {GoldAmount}. Current: {_gold.Amount}");
    }
    
    private bool VerifyAndCreateItem(out ItemData newItem)
    {
        newItem = _itemFactory.Create(TargetId, TargetGrade); 

        if (newItem == null)
        {
            Debug.LogError($"Failed to create item. ID: {TargetId}");
            return false;
        }

        return true;
    }
}
