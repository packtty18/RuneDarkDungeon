using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMove : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _rotationSpeed = 10f;

    private NavMeshAgent _agent;
    private Transform _target;

    // 외부 제어용 (Battle / Attack / Cutscene)
    private bool _isPaused;

    public bool IsMoving => IsAgentActive && !_isPaused;

    [SerializeField] private bool _onTest = false;

    private bool IsAgentActive => _agent != null && _agent.enabled;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!IsMoving || _target == null)
            return;

        RotateToTarget();
    }

    public void Init()
    {
        if (_agent == null)
        {
            _agent = GetComponent<NavMeshAgent>();
            Debug.Log("EnemyMove :Agent Not Exist", this);
        }

        SetAgentSetting();
        EnableAgent();

        _isPaused = false;
        _target = null;

        Debug.Log("[EnemyMove] Initialized", this);
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
        Debug.Log("[EnemyMove] 타겟 지정", this);
    }


    [Button, ShowIf(nameof(_onTest))]
    public void StartMove()
    {
        if (_target == null || _isPaused)
            return;

        EnableAgent();
        _agent.SetDestination(_target.position);

        Debug.Log("[EnemyMove] Move 시작", this);
    }

    [Button, ShowIf(nameof(_onTest))]
    public void StopMove()
    {
        if (!IsAgentActive)
            return;

        _agent.ResetPath();
        Debug.Log("[EnemyMove] Move 종료", this);
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

        if (IsAgentActive)
            //_agent.ResetPath();

        Debug.Log("[EnemyMove] 에이전트 정지", this);
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
            //_agent.SetDestination(_target.position);
        }

        Debug.Log("[EnemyMove] 에이전트 재시작", this);
    }

    [Button, ShowIf(nameof(_onTest))]
    public void ResetAgent()
    {
        EnableAgent();
        _agent.ResetPath();

        _isPaused = false;
        _target = null;

        Debug.Log("[EnemyMove] 리셋", this);
    }

    private void RotateToTarget()
    {
        Vector3 direction = _target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

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
        _agent.Warp(transform.position);
    }
}
