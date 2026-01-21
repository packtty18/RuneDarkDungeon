using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMove : MonoBehaviour
{
    private const float NAVMESH_SAMPLE_RADIUS = 5f;
    [Header("Settings")]
    [SerializeField] private float _rotationSpeed = 10f;

    private NavMeshAgent _agent;
    private Transform _target;

    [SerializeField] private bool _isPaused;

    [SerializeField] private bool _canRotate;

    [SerializeField] private bool _onTest = false;

    private bool IsAgentActive => _agent != null && _agent.enabled;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_target == null)
            return;

        if(_canRotate)
        {
            RotateToTarget();
        }
    }

    public void SetAbleToRatate(bool enable)
    {
        _canRotate = enable;
    }

    public void Init()
    {
        if (_agent == null)
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        // Agent 완전 초기화
        if (_agent != null)
        {
            if (_agent.enabled)
            {
                _agent.ResetPath();
            }
            _agent.enabled = false;
        }

        SetAgentSetting();

        _isPaused = false;
        _target = null;
        _canRotate = false;
    }

    public void MoveByDirection(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;

        LookAt(direction);
    }

    public void SetAgentSetting()
    {
        //이동속도
        //회전속도
        //가속도
        //멈추는 거리 (공격거리)
        //AutoBraking여부 (목표위치를 계산하여 감속)
    }

    [Button, ShowIf(nameof(_onTest))]
    public void SetTarget(Transform target)
    {
        _target = target;
    }


    [Button, ShowIf(nameof(_onTest))]
    public void StartMove()
    {
        if (_target == null || _isPaused)
            return;

        EnableAgent();

        // Ensure agent is on NavMesh
        if (!_agent.isOnNavMesh)
        {
            TryWarpToNearestNavMesh();
        }

        _agent.SetDestination(_target.position);
    }
    private void TryWarpToNearestNavMesh()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, NAVMESH_SAMPLE_RADIUS, NavMesh.AllAreas))
        {
            Debug.Log($"[EnemyMove] Warped to nearest NavMesh at {hit.position}");
            _agent.Warp(hit.position);
        }
        else
        {
            GetComponent<EnemyController>().Dead();
            Debug.LogWarning("[EnemyMove] Failed to find nearby NavMesh.");
        }
    }

    [Button, ShowIf(nameof(_onTest))]
    public void StopMove()
    {
        if (!IsAgentActive)
            return;

        _agent.ResetPath();
    }
    public void SetPaused(bool paused)
    {
        if (_isPaused == paused)
            return;

        if (paused)
            PauseAgent();
        else
            ResumeAgent();
    }

    //일시정지와 재시작, 공격 애니메이션 전 실행후 종료 후 재시작 용도
    [Button, ShowIf(nameof(_onTest))]
    public void PauseAgent()
    {
        if (_isPaused)
            return;

        _isPaused = true;

        DisableAgent();
    }

    [Button, ShowIf(nameof(_onTest))]
    public void ResumeAgent()
    {
        if (!_isPaused)
            return;

        _isPaused = false;

        if (_target != null)
        {
            EnableAgent();
        }
    }

    [Button, ShowIf(nameof(_onTest))]
    public void ResetAgent()
    {
        if (_agent != null && _agent.enabled)
        {
            _agent.ResetPath();
            _agent.enabled = false;
        }

        _isPaused = false;
        _target = null;
        _canRotate = false;
    }

    private void RotateToTarget()
    {
        Vector3 direction = _target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        LookAt(direction);
    }

    private void LookAt(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );

    }

    private void EnableAgent()
    {
        if (IsAgentActive)
            return;

        _agent.enabled = true;
        // Warp를 통해 현재 위치를 Agent에 명확히 설정
        _agent.Warp(transform.position);
        _canRotate = false;
    }

    private void DisableAgent()
    {
        if (!IsAgentActive)
            return;

        _agent.ResetPath();
        _agent.enabled = false;
        _canRotate = true;
    }
}
