using UnityEngine;
using UnityEngine.UI;

public class UI_DragIcon : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private Image _iconImage;
    
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
    
    public void Show(Sprite icon)
    {
        transform.position = Input.mousePosition;
        _iconImage.sprite = icon;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _iconImage.sprite = null;
        gameObject.SetActive(false);
    }
}
