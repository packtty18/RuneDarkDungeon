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
    
    public void Show(Sprite icon)
    {
        _iconImage.sprite = icon;
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
