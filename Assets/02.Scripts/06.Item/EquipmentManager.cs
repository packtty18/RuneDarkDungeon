using UnityEngine;

public class EquipmentManager : IEquipmentManager
{
    private IEquipment _equipment;
    private IInventory _inventory;
    
    public EquipmentManager(IEquipment equipment, IInventory inventory)
    {
        _equipment = equipment;
        _inventory = inventory;
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
}
