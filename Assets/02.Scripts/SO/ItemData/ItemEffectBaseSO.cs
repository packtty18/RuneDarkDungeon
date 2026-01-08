using UnityEngine;

public abstract class ItemEffectBaseSO : ScriptableObject
{
    public virtual void OnUse(GameObject user) { }

    public virtual void OnEquip(GameObject user) { }
    
    public virtual void OnUnequip(GameObject user) { }
}
