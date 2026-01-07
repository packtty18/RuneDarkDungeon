using UnityEngine;
using UnityEngine.UI;

public class UI_DragIcon : MonoBehaviour
{
    [SerializeField] private Image _iconImage;

    private void Awake()
    {
        Hide();   
    }
    
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
    
    public void Show(ItemSO item)
    {
        _iconImage.sprite = item.Icon;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _iconImage.sprite = null;
        gameObject.SetActive(false);
    }

    public void SetPositionOnMouse(Vector3 position)
    {
        transform.position = position;
    }
}
