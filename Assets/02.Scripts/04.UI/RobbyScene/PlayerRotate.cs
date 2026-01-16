using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField]
    private LayerMask _mask;

    private Camera _camera;

    public float RotationSpeed = 15f;
    private float yaw = 0f;
    private float pitch = 0f;

    private bool _isRotating = false;

    void Awake()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Click))
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.nearClipPlane));
            Vector3 dir = (mousePos - _camera.transform.position).normalized;

            Debug.DrawRay(_camera.transform.position, dir * 100.0f, Color.red, 1.0f);
            //카메라위치에서 투영면까지 가는 방향 벡터

            RaycastHit hit;
            Ray ray = new Ray(_camera.transform.position, dir);
            if (Physics.Raycast(ray, out hit, 100.0f, _mask))
            {
                Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
                _isRotating = true;
            }
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Click))
        {
            if (!_isRotating)
            {
                return;
            }
            float mouseX = Input.GetAxis("Mouse X");
            
            yaw -= mouseX * RotationSpeed * Time.deltaTime;

            transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
        }
        if (InputManager.Instance.GetKeyUp(EGameKeyType.Click))
        {
            _isRotating = false;
        }
    }
}
