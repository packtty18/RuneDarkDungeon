using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Item/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField, TextArea] private string _tooltip;
    
    public int ID => _id;
    public Sprite Icon => _icon;
    public string Name => _name;
    public string Tooltip => _tooltip;
}
