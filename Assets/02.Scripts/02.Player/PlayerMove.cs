using System;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    private InputManager _inputManager;
    private CharacterController _controller;
    private Player _player;
    private PlayerAnimator _animator;

    private float _groundCheckRadius;
    private float _groundCheckOffset = 0f;
    [SerializeField] private LayerMask _groundLayers;

    private float _runSpeedMultiplier = 2f;

    private float _currentSpeed;
    private float _walkSpeed;

    private float _gravity;
    private float _verticalVelocity;

    [SerializeField] private int _maxJumpCount = 2; 
    private int _currentJumpCount;
    private float _jumpVelocity;
    private bool _jumpRequested = false;
    private float _landOffset = 0.5f;
    public bool IsRunning { get; private set; } = false;
    public bool IsGrounded { get; private set; } 

    private Action<float> _onMoveSpeedChanged;
    void Start()
    {  
        Initialize();
        SubscribeEvents();
    }

    private void Update()
    {
        GroundedCheck();
        RunInput();
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

        _walkSpeed = _player.GetSpeed();
        _currentSpeed = _walkSpeed;

        _gravity = _player.GetGravity();

        _groundCheckRadius = _controller.radius * 0.9f;

        _currentJumpCount = 0;
        _jumpVelocity = Mathf.Sqrt(_player.GetJumpVelocity() * -2f * _gravity);

        _animator = GetComponent<PlayerAnimator>();
    }

    private void SubscribeEvents()
    {
        _onMoveSpeedChanged = HandleMoveSpeedChanged;
        _player.SubscribeSpeed(_onMoveSpeedChanged);
    }

    private void UnSubscribeEvents()
    {
        _player.UnsubscribeSpeed(_onMoveSpeedChanged);
    }
    
    private void Movement()
    {
        Vector3 moveDirection = GetMoveDirection();
        if (moveDirection.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.2f);
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
                _verticalVelocity = _jumpVelocity;
                _currentJumpCount++;
                _jumpRequested = true;
            }       
        }

        if (_currentJumpCount > 0 && _verticalVelocity < 0)
        {
            if (GroundCheckInDirection(Vector3.down, _landOffset))
            {
                Debug.Log("Landed");
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

    private void RunInput()
    {
        bool shouldRun = _inputManager.GetKey(EGameKeyType.Run);

        if (_currentJumpCount > 0)
        {
            return;
        }

        if (shouldRun != IsRunning)
        {
            IsRunning = shouldRun;
            UpdateCurrentSpeed();
        }
    }
    private void HandleMoveSpeedChanged(float obj)
    {
        _walkSpeed = obj;
        UpdateCurrentSpeed();
    }

    private void UpdateCurrentSpeed()
    {
        _currentSpeed = IsRunning ? _walkSpeed * _runSpeedMultiplier : _walkSpeed;
    }


    #region IsGrounded Check
    private void GroundedCheck()
    {
        Vector3 spherePosition = GetSpherePosition();
        
        IsGrounded = Physics.CheckSphere(
            spherePosition, 
            _groundCheckRadius, 
            _groundLayers, 
            QueryTriggerInteraction.Ignore
            );

        //점프 후 착지했을 때
        if (IsGrounded && _currentJumpCount > 0)
        {
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
