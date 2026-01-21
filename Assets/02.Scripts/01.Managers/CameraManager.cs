using Unity.Cinemachine;
using UnityEngine;


public class CameraManager : LocalSingleton<CameraManager>
{
    [Header("모드 별 카메라")]
    [SerializeField]
    private CinemachineCamera _defaultCamera;

    [SerializeField]
    private CinemachineCamera _caveCamera;
    [SerializeField]
    private float _caveBlendTime = 0.8f;

    [SerializeField]
    private CinemachineCamera _deathCamera;

    private CinemachineBrain _brain;

    private ECameraMode _cameraMode;

    public ECameraMode CameraMode => _cameraMode;

    private int _activePriority = 10;
    private int _inactivePriority = 0;

    protected override void Awake()
    {
        base.Awake();

        Camera.main.TryGetComponent<CinemachineBrain>(out _brain);
    }
    private void Start()
    {
        SetCameraMode(ECameraMode.Default);
    }
    public void SetCameraMode(ECameraMode cameraMode)
    {
        _cameraMode = cameraMode;

        switch (_cameraMode)
        {
            case ECameraMode.Cave:
                _brain.DefaultBlend.Time = _caveBlendTime;
                break;
        }

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
