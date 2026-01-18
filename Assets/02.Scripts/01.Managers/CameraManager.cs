using Unity.Cinemachine;
using UnityEngine;
using static UnityEditor.SceneView;

public class CameraManager : LocalSingleton<CameraManager>
{
    [Header("모드 별 카메라")]
    [SerializeField]
    private CinemachineCamera _defaultCamera;

    [SerializeField]
    private CinemachineCamera _caveCamera;

    private ECameraMode _cameraMode;

    public ECameraMode CameraMode => _cameraMode;

    private int _activePriority = 10;
    private int _inactivePriority = 0;


    private void Start()
    {

        SetCameraMode(ECameraMode.Default);
    }
    public void SetCameraMode(ECameraMode cameraMode)
    {
        _cameraMode = cameraMode;

        _defaultCamera.Priority = _cameraMode == ECameraMode.Default ? _activePriority : _inactivePriority;
        _caveCamera.Priority = _cameraMode == ECameraMode.Cave ? _activePriority : _inactivePriority;
    }

    public void CameraShake(float intensity, float duration)
    {
        CinemachineCamera activeCamera = _cameraMode == ECameraMode.Default
            ? _defaultCamera
            : _caveCamera;

        activeCamera.GetComponent<CameraShakeController>()?.CameraShake(intensity, duration);
    }

}
