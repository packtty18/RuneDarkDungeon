using Sirenix.OdinInspector;
using UnityEngine;

//활성화 상태에서 플레이어와 트리거되면
//지정된 스폰매니저의 스폰페이즈를 실행한다.
[RequireComponent(typeof(Collider))]
public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] private EnemySpawnManager _conntectedManager;
    [SerializeField] private Collider _collider;
    [SerializeField] private bool _isActivate = false;
    private bool _triggered;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        _isActivate = false;

    }

    //해당 함수는 이전 스테이지의 스폰매니저의 OnAllPhaseCompleted의 이벤트로 추가한다.
    [Button]
    public void ActiveSwitch()
    {
        _collider.enabled = true;
        _isActivate = true;
        Debug.Log($"[SpawnTrigger] {name} 트리거가 활성화되었습니다.");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered || !_isActivate)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("[SpawnTrigger] 플레이어 감지. 페이즈를 실행");

        _conntectedManager.PlayCurrentPhase();

        _triggered = true;
    }
}
