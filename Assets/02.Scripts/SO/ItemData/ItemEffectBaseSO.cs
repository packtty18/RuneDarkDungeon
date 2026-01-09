using UnityEngine;

public abstract class ItemEffectBaseSO : ScriptableObject
{
    public virtual void OnUse(GameObject user, EItemGrade grade) { }

    public virtual void OnEquip(GameObject user, EItemGrade grade) { }
    
    public virtual void OnUnequip(GameObject user, EItemGrade grade) { }
}
