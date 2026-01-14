using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        Debug.Log("[WorldUIBillboard] Initialized");
    }

    private void LateUpdate()
    {
        if (_mainCamera == null)
            return;

        transform.rotation = _mainCamera.transform.rotation;
    }
}
