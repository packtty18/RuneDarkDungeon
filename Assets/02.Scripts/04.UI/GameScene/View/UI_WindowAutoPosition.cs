using UnityEngine;

public class UI_WindowAutoPosition : MonoBehaviour
{
    [SerializeField] private RectTransform _parentTransform;
    [SerializeField] private RectTransform _rectTransform;
    
    private readonly Vector3[] _corners = new Vector3[4];
    
    private void OnEnable()
    {
        _rectTransform.GetWorldCorners(_corners);
        
        float deltaX = 0f;
        float deltaY = 0f;
        
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        if (_corners[0].x < 0f)
        {
            deltaX = 0f - _corners[0].x;
        }
        else if (_corners[2].x > screenWidth)
        {
            deltaX = screenWidth - _corners[2].x;
        }

        if (_corners[0].y < 0f)
        {
            deltaY = 0f - _corners[0].y;
        }
        else if (_corners[2].y > screenHeight)
        {
            deltaY = screenHeight - _corners[2].y;
        }

        if (deltaX != 0f || deltaY != 0f)
        {
            Vector3 position = _parentTransform.position;
            position.x += deltaX;
            position.y += deltaY;
            _parentTransform.position = position;
        }
    }
}
