using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



[RequireComponent(typeof(PlayerAnimator))]
public class PlayerMove : MonoBehaviour
{
    private InputManager _inputManager;
    private CharacterController _controller;
    private PlayerStats _playerStats;
    private PlayerAnimator _animator;
    private PlayerStateMachine _stateMachine;
    private Camera _camera;

    [Header("이동")]
    [SerializeField] private float _turnRate = 3f;

    private float _groundCheckRadius;
    private float _groundCheckOffset = 0.1f;
    [Header("그라운드 감지")]
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private LayerMask _walkableLayer;


    [Header("속도")]
    [SerializeField] private float _runSpeedMultiplier = 2f;
    [SerializeField] private float _speedChangeRate = 5;

    private float _speedOffset = 0.2f;
    private float _currentSpeed;
    private float _walkSpeed;
    private float _runSpeed;

    private float _gravity;
    private float _verticalVelocity;

    [Header("점프")]
    [Tooltip("점프 가능 횟수")]
    [SerializeField] private int _maxJumpCount = 2; 
    private int _currentJumpCount;
    private float _jumpVelocity;
    private bool _jumpRequested = false;
    private float _landOffset = 0.7f;
    private bool _isJumping = false;

    private float _dodgeSpeed = 15f;
    public bool IsGrounded { get; private set; }
    public bool ShouldRun { get; private set; }

    public event Action<bool> OnIsJumpingChanged;
    public event  Action<float> OnMoveSpeedChanged;
    public event Action<bool> OnCanMoveChanged;
    public event Action OnDashEnd;

    private void Awake()
    {
        _animator = GetComponent<PlayerAnimator>();
        _controller = GetComponent<CharacterController>();
        _playerStats = GetComponent<PlayerStats>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _camera = Camera.main;
    }
    private void Start()
    {
        Initialize();
        SubscribeEvents();
    }

    private void Update()
    {
        GroundedCheck();
        Dodge();
        Movement();
        ApplyJump();
        ApplyGravity();
    }

    private void OnDestroy()
    {
        UnSubscribeEvents();
    }

    private void Initialize()
    {
        _inputManager = InputManager.Instance;
        HandleMoveSpeedChanged(_playerStats.MoveSpeed.Current);
        _currentSpeed = 0;

        _gravity = _playerStats.Gravity.Value;

        _groundCheckRadius = _controller.radius * 0.9f;

        _currentJumpCount = 0;
        _jumpVelocity = Mathf.Sqrt(_playerStats.JumpPower.Value * -2f * _gravity);
    }

    private void SubscribeEvents()
    {
        OnMoveSpeedChanged = HandleMoveSpeedChanged;
        _playerStats.MoveSpeed.Subscribe(OnMoveSpeedChanged);
    }

    private void UnSubscribeEvents()
    {
        _playerStats.MoveSpeed.Subscribe(OnMoveSpeedChanged);
    }
    
    private void Movement()
    {
        Vector3 moveDirection = GetMoveDirection();
        float moveScale = moveDirection.magnitude;

        SpeedUpdate(moveScale);

        if (moveScale > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), _turnRate * Time.deltaTime);
            _controller.Move(transform.forward * _currentSpeed * Time.deltaTime);
        }      
    }

    private void Dodge()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Dodge))
        {
            if (!_stateMachine.CanReceiveMoveInput() || !IsGrounded) return;
            _animator.SetDodge(true);
        }  
        if (_stateMachine.CurrentActionState == EActionState.Dodge)
        {
            _controller.Move(-transform.forward * Time.deltaTime * _dodgeSpeed);
        }
    }

    public void OnDodgeStart()
    {
        _stateMachine.SetActionState(EActionState.Dodge);
    }
    public void OnDodgeFinish()
    {
        _stateMachine.SetActionState(EActionState.None);
        _animator.SetDodge(false);
    }

    private Vector3 GetMoveDirection()
    {
        if (!_stateMachine.CanReceiveMoveInput()) 
            return Vector3.zero;

        Vector3 direction = Vector3.zero;
        if (_inputManager.GetKey(EGameKeyType.Front))
        {
            direction += _camera.transform.forward;
        }
        if (_inputManager.GetKey(EGameKeyType.Back))
        {
            direction -= _camera.transform.forward;
        }
        if (_inputManager.GetKey(EGameKeyType.Left))
        {
            direction -= _camera.transform.right;
        }
        if (_inputManager.GetKey(EGameKeyType.Right))
        {
            direction += _camera.transform.right;
        }

        direction.y = 0;
        return direction.normalized;
    }

    private void ApplyJump()
    {
        if (_inputManager.GetKeyDown(EGameKeyType.Jump))
        {
            if (!_stateMachine.CanReceiveMoveInput()) return;
            if (_currentJumpCount < _maxJumpCount)
            {
                _verticalVelocity = _jumpVelocity;
                _currentJumpCount++;
                _jumpRequested = true;
                OnIsJumpingChanged?.Invoke(IsJumping());
            }
        }

        if (!IsGrounded && _verticalVelocity < 0)
        {
            if (GroundCheckInDirection(Vector3.down, _landOffset, _walkableLayer))
            {
                _animator.SetJump(false);
            }

        }
    }

    public bool IsJumping()
    {
        if (_currentJumpCount > 0)
        {
            return true;
        }
        return false;
    }

    private void ApplyGravity()
    {
        if (IsGrounded && !_jumpRequested)
        {
            _verticalVelocity = -2;
        }
        else
        {
            _verticalVelocity += _gravity * Time.deltaTime;
            if (_jumpRequested)
            {
                _animator.SetJump(true);
                _jumpRequested = false;
            }
        }
        _controller.Move(Vector3.up * _verticalVelocity * Time.deltaTime);
    }

    private void SpeedUpdate(float moveScale)
    {
        if (_currentJumpCount > 0)
        {
            return;
        }

        ShouldRun = _inputManager.GetKey(EGameKeyType.Run);

        float targetSpeed = ShouldRun ? _runSpeed : _walkSpeed;

        if (moveScale < 0.1f)
        {
            targetSpeed = 0f;
        }

        if (_currentSpeed < targetSpeed - _speedOffset || _currentSpeed > targetSpeed + _speedOffset)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, _speedChangeRate * Time.deltaTime);
            _animator.SetSpeedRatio(CalculateBlendTreeParameter());
        }
        else
        {
            _currentSpeed = targetSpeed;
            _animator.SetSpeedRatio(CalculateBlendTreeParameter());
        }
    }

    private float CalculateBlendTreeParameter()
    {
        if (_currentSpeed < 0.01f)
        {
            return 0;
        }

        if (_currentSpeed < _walkSpeed)
        {
            return _currentSpeed/_walkSpeed;
        }

        else
        {
            float excess = _currentSpeed - _walkSpeed;
            float runRange = _runSpeed - _walkSpeed;
            if (Mathf.Approximately(runRange, 0f)) return 1f;
            return 1 + excess / runRange;
        }
    }
    private void HandleMoveSpeedChanged(float obj)
    {
        _walkSpeed = obj;
        _runSpeed = _walkSpeed * _runSpeedMultiplier;
    }


    public void KnockBack(Vector3 direction)
    {
        _controller.Move(direction);
    }

    #region Dash
    public void StartGroundDash(float dashAngle, float dashSpeed)
    {
        if (!IsJumping()) return;

        _currentJumpCount = _maxJumpCount;
        _verticalVelocity = 0f;

        // 현재 입력 방향 또는 플레이어가 보는 방향
        Vector3 moveInput = GetMoveDirection();
        Vector3 horizontal = moveInput.magnitude > 0.1f ? moveInput : transform.forward;

        Vector3 dashDirection = CalculateDashDirection(horizontal, dashAngle);
        StartCoroutine(DashToGroundCoroutine(dashDirection, dashSpeed));
    }

    private Vector3 CalculateDashDirection(Vector3 horizontalDir, float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;

        Vector3 direction = horizontalDir.normalized * Mathf.Cos(angleRad)
                            + Vector3.down * Mathf.Sin(angleRad);

        return direction.normalized;
    }

    private IEnumerator DashToGroundCoroutine(Vector3 direction, float speed)
    {
        float currentSpeed = 0;
        float acceleration = 100f; // 가속도
        _controller.excludeLayers = _controller.excludeLayers | (_walkableLayer & ~_groundLayers);

        while (!GroundCheckInDirection(Vector3.down, 0, _groundLayers))
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, speed, acceleration * Time.deltaTime);
            _controller.Move(direction * speed * Time.deltaTime);
 
            yield return null;
        }
        _controller.excludeLayers = _controller.excludeLayers & ~_walkableLayer;
        OnDashEnd?.Invoke();
    }

    #endregion
    #region IsGrounded Check
    private void GroundedCheck()
    {
        Vector3 spherePosition = GetSpherePosition();

        IsGrounded = _verticalVelocity <= 0 && Physics.CheckSphere(
            spherePosition,
            _groundCheckRadius,
            _walkableLayer,
            QueryTriggerInteraction.Ignore
        );

        // 점프 후 착지했을 때.
        if (IsGrounded && _currentJumpCount > 0)
        {
            _currentJumpCount = 0;
            OnIsJumpingChanged?.Invoke(IsJumping());
            //_animator.SetJump(false);

        }
    }

    private Vector3 GetSpherePosition()
    {
        return new Vector3(
            transform.position.x,
            transform.position.y - _groundCheckOffset + _groundCheckRadius,
            transform.position.z);
    }

    public bool GroundCheckInDirection(Vector3 direction, float distance, LayerMask layer)
    {
        Vector3 spherePosition = GetSpherePosition() + direction * distance;

         return Physics.CheckSphere(
                spherePosition,
                _groundCheckRadius,
                layer,
                QueryTriggerInteraction.Ignore
                );
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmo();
    }
    private void DrawGizmo()
    {
        Vector3 spherePosition = GetSpherePosition();

        Gizmos.color = IsGrounded
            ? new Color(0f, 1f, 0f, 0.5f)
            : new Color(1f, 0f, 0f, 0.5f);

        Gizmos.DrawSphere(spherePosition, _groundCheckRadius);

        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(spherePosition, _groundCheckRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, spherePosition);
    }

    #endregion
}
