using Unity.Cinemachine;
using UnityEngine;

public class BossCameraTargetPresenter : MonoBehaviour
{
    [SerializeField] private BossContext _bossContext;

    private CinemachineCamera _camera;

    private void Awake()
    {
        if (_bossContext.Boss != null)
            Bind();

        _bossContext.Subscribe(Bind);
        _camera = GetComponent<CinemachineCamera>();
    }

    void Bind()
    {
        _camera.Target.TrackingTarget = _bossContext.Boss.transform;
    }

    private void OnDestroy()
    {
        _bossContext.Unsubscribe(Bind);
    }
}
