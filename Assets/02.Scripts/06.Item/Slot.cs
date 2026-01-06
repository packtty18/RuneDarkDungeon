using UnityEngine;

public class Slot : MonoBehaviour
{
    private ItemData _item;
    private ItemSO _info;
    
    public bool IsEmpty =>  _item == null;

    public void SetItem(ItemData item, ItemSO info)
    {
        _item = item;
        _info = info;
    }

    public void Clear()
    {
        _item = null;
        _info = null;
    }
}
