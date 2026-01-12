using UnityEngine;

public class EquipmentManager : IEquipmentManager
{
    private IEquipment _equipment;
    private IInventory _inventory;
    private ItemDatabaseSO _itemDB;
    
    public EquipmentManager(IEquipment equipment, IInventory inventory, ItemDatabaseSO itemDB)
    {
        _equipment = equipment;
        _inventory = inventory;
        _itemDB = itemDB;
    }

    public void EquipItem(ESkillSlot slot, ItemData newItem)
    {
        _inventory.Remove(newItem);
        ItemData oldItem = _equipment.Equip(slot, newItem);

        if (oldItem == null) return;
        _inventory.Add(oldItem);
    }

    public void UnEquipItem(ESkillSlot slot)
    {
        ItemData item = _equipment.UnEquip(slot);

        if (item == null) return;
        _inventory.Add(item);
    }

    public bool UseItem(GameObject user, ESkillSlot slot, out float coolTime)
    {
        coolTime = 0;
        var item = _equipment.GetItem(slot);
        if (item == null) return false;
        
        _itemDB.UseItem(user, item, out coolTime);
        return true;
    }
}
