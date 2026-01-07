using System;
using UnityEngine;


[RequireComponent(typeof(PlayerAnimator))]
public class PlayerMove : MonoBehaviour
{
    private InputManager _inputManager;
    private CharacterController _controller;
    private Player _player;
    private PlayerAnimator _animator;

    [Header("이동")]
    [SerializeField] private float _turnRate = 3f;

    private float _groundCheckRadius;
    private float _groundCheckOffset = 0.1f;
    [Header("그라운드 감지")]
    [SerializeField] private LayerMask _groundLayers;

    [Header("속도")]
    [SerializeField] private float _runSpeedMultiplier = 2f;
    [SerializeField] private float _speedChangeRate = 5;

    private float _speedOffset = 0.1f;
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
    private float _landOffset = 0.5f;
    private bool _isJumping = false;
    public bool IsGrounded { get; private set; }

    public bool ShouldRun { get; private set; }
    public bool IsJumping 
    {   get { return _isJumping; }
        private set
        {
            _isJumping = value;
            OnIsJumpingChanged?.Invoke(value);
        }
    }

    public event Action<bool> OnIsJumpingChanged;
    public event  Action<float> OnMoveSpeedChanged;

    void Start()
    {  
        Initialize();
        SubscribeEvents();
    }

    private void Update()
    {
        GroundedCheck();
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

        _controller = GetComponent<CharacterController>();
        _player = GetComponent<Player>();

        HandleMoveSpeedChanged(_player.GetSpeed());
        _currentSpeed = 0;

        _gravity = _player.GetGravity();

        _groundCheckRadius = _controller.radius * 0.9f;

        _currentJumpCount = 0;
        _jumpVelocity = Mathf.Sqrt(_player.GetJumpVelocity() * -2f * _gravity);
        IsJumping = false;

        _animator = GetComponent<PlayerAnimator>();
    }

    private void SubscribeEvents()
    {
        OnMoveSpeedChanged = HandleMoveSpeedChanged;
        _player.SubscribeSpeed(OnMoveSpeedChanged);
    }

    private void UnSubscribeEvents()
    {
        _player.UnsubscribeSpeed(OnMoveSpeedChanged);
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

    private Vector3 GetMoveDirection()
    {
        Vector3 direction = Vector3.zero;
        if (_inputManager.GetKey(EGameKeyType.Front))
        {
            direction += Vector3.forward;
        }
        if (_inputManager.GetKey(EGameKeyType.Back))
        {
            direction -= Vector3.forward;
        }
        if (_inputManager.GetKey(EGameKeyType.Left))
        {
            direction -= Vector3.right;
        }
        if (_inputManager.GetKey(EGameKeyType.Right))
        {
            direction += Vector3.right;
        }
        return direction.normalized;
    }

    private void ApplyJump()
    {
        if (_inputManager.GetKeyDown(EGameKeyType.Jump))
        {
            if (_currentJumpCount < _maxJumpCount)
            {
                if (_currentJumpCount == 0)
                {
                    IsJumping = true;
                }
                _verticalVelocity = _jumpVelocity;
                _currentJumpCount++;
                _jumpRequested = true;
            }       
        }

        if (_currentJumpCount > 0 && _verticalVelocity < 0)
        {
            if (GroundCheckInDirection(Vector3.down, _landOffset))
            {
                _animator.SetJump(false);
            }

        }
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

        if (_inputManager.GetKeyDown(EGameKeyType.Run))
        {
            ShouldRun = true;
        }

        if (_inputManager.GetKeyUp(EGameKeyType.Run))
        {
            ShouldRun = false;
        }


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

    public void SetShouldRun (bool shouldRun)
    {
        ShouldRun = shouldRun;
    }

    #region IsGrounded Check
    private void GroundedCheck()
    {
        Vector3 spherePosition = GetSpherePosition();

        IsGrounded = _verticalVelocity <= 0 && Physics.CheckSphere(
            spherePosition,
            _groundCheckRadius,
            _groundLayers,
            QueryTriggerInteraction.Ignore
        );

        // 점프 후 착지했을 때.
        if (IsGrounded && _currentJumpCount > 0)
        {
            IsJumping = false;
            _currentJumpCount = 0;
        }
    }

    private Vector3 GetSpherePosition()
    {
        return new Vector3(
            transform.position.x,
            transform.position.y - _groundCheckOffset + _groundCheckRadius,
            transform.position.z);
    }

    public bool GroundCheckInDirection(Vector3 direction, float distance)
    {
        Vector3 spherePosition = GetSpherePosition() + direction * distance;

         return Physics.CheckSphere(
                spherePosition,
                _groundCheckRadius,
                _groundLayers,
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
