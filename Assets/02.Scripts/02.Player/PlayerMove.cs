using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    private CharacterController _Controller;
    private Player _Player;

    private float _groundCheckRadius;
    private float _groundCheckOffset = 0.1f;
    [SerializeField] private LayerMask _groundLayers;

    private float _runSpeedMultiplier = 2f;

    private float _currentSpeed;
    private float _walkSpeed;

    private float _gravity;
    private float _verticalVelocity;

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
        RunInput();
        Movement();
        ApplyGravity();
        GroundedCheck();
        ApplyJump();
    }

    private void OnDestroy()
    {
        UnSubscribeEvents();
    }

    private void Initialize()
    {
        _Controller = GetComponent<CharacterController>();
        _Player = GetComponent<Player>();

        _walkSpeed = _Player.PlayerStats.MoveSpeed.Current;
        _currentSpeed = _walkSpeed;

        _gravity = _Player.PlayerStats.Gravity.Value;

        _groundCheckRadius = _Controller.radius * 0.9f;
    }

    private void SubscribeEvents()
    {
        _onMoveSpeedChanged = HandleMoveSpeedChanged;
        _Player.PlayerStats.MoveSpeed.Subscribe(_onMoveSpeedChanged);
    }

    private void UnSubscribeEvents()
    {
        _Player.PlayerStats.MoveSpeed.Unsubscribe(_onMoveSpeedChanged);
    }
    
    private void Movement()
    {
        Vector3 moveDirection = GetMoveDirection();
        
        _Controller.Move(moveDirection * _currentSpeed * Time.deltaTime);
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 direction = Vector3.zero;
        if (InputManager.Instance.GetKey(EGameKeyType.Front))
        {
            direction += Vector3.forward;
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Back))
        {
            direction += Vector3.back;
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Left))
        {
            direction += Vector3.left;
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Right))
        {
            direction += Vector3.right;
        }
        return direction.normalized;
    }

    private void ApplyJump()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Jump))
        {
            if (IsGrounded)
            {
                _verticalVelocity = Mathf.Sqrt(_Player.PlayerStats.JumpPower.Value * -2f * _gravity);
            }       
        }
    }

    private void ApplyGravity()
    {
        if (_Controller.isGrounded)
        {
            _verticalVelocity = 0;
        }
        else
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
        _Controller.Move(Vector3.up * _verticalVelocity * Time.deltaTime);
    }

    private void RunInput()
    {
        bool shouldRun = InputManager.Instance.GetKey(EGameKeyType.Run);

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
