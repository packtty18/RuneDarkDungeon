using Unity.Cinemachine;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Camera _renderCamera;

    private void Awake()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        _renderCamera = brain != null ? brain.OutputCamera : Camera.main;
    }

    private void LateUpdate()
    {
        if (_renderCamera == null)
            return;

        Vector3 dir = transform.position - _renderCamera.transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
