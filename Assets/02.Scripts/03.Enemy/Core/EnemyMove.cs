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

    private bool _canRotate;

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

        SetAgentSetting();

        _isPaused = false;
        _target = null;
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
        _agent.SetDestination(_target.position);

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
        EnableAgent();
        _agent.ResetPath();

        _isPaused = false;
        _target = null;
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
