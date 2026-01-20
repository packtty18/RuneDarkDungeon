using Unity.Cinemachine;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class CameraTargetPresenter : MonoBehaviour
{
    [SerializeField] private PlayerContext _playerContext;

    private CinemachineCamera _camera;

    private void Awake()
    {
        if (_playerContext.Player != null)
            Bind();

        _playerContext.Subscribe(Bind);
        _camera = GetComponent<CinemachineCamera>();
    }

    void Bind()
    {
        _camera.Target.TrackingTarget = _playerContext.Player.transform;
    }

    private void OnDestroy()
    {
        _playerContext.Unsubscribe(Bind);
    }
}
