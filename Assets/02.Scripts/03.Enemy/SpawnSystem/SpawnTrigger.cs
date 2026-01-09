using Sirenix.OdinInspector;
using UnityEngine;

// 활성화 상태에서 플레이어와 트리거되면
// 지정된 스폰매니저의 스폰 페이즈를 실행한다.
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] private EnemySpawnManager _connectedManager;
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private bool _isActivate = false;

    private bool _triggered;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<Renderer>();

        _collider.enabled = false;
        _isActivate = false;

        Debug.Log($"[SpawnTrigger] {name} 초기화");
    }

    [Button]
    public void ActiveSwitch()
    {
        _collider.enabled = true;
        _isActivate = true;

        SetColor(Color.green);
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

        Debug.Log("[SpawnTrigger] 플레이어 감지");

        _connectedManager.PlayCurrentPhase();
        _triggered = true;
    }

    private void SetColor(Color targetColor)
    {
        Color current = _renderer.material.color;
        targetColor.a = current.a; 
        _renderer.material.color = targetColor;
    }
}
