using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterActiveTrigger : MonoBehaviour
{
    //해당 매니저가 종료될경우 액티브함
    private MeshRenderer _renderer;
    private Collider _collider;

    [SerializeField] private Material _activeMaterial;
    [SerializeField] private Material _deactiveMaterial;

    [SerializeField] private bool _initActive = false;
    [HideIf(nameof(_initActive))]
    [SerializeField] private EnemySpawnManager _targetManager;
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();

        if(_initActive)
        {
            ActiveTrigger();
            return;
        }

        _targetManager.OnAllPhaseCompleted.Subscribe(ActiveTrigger);
        _renderer.material = _deactiveMaterial;
        _collider.enabled = false;
    }

    private void ActiveTrigger()
    {
        if(_targetManager != null)
        {
            _targetManager.OnAllPhaseCompleted.Unsubscribe(ActiveTrigger);
        }
        
        _renderer.material = _activeMaterial;
        _collider.enabled = true;
    }

    private void ActiveMonster()
    {
        if(!BattleManager.IsExist())
        {
            return;
        }
        //다음 스테이지 시작됨
        BattleManager.Instance.NotifyActiveStage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ActiveMonster();
            Util.ObjectDestroy(gameObject);
        }
    }
}
