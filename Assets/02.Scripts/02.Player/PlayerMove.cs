using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    private CharacterController _Controller;
    private Player _Player;


    private float _runSpeedMultiplier = 2f;

    private float _currentSpeed;
    private float _walkSpeed;
    private bool _isRunning = false;

    private Action<float> _onMoveSpeedChanged;
    void Start()
    {
        InitializeComponents();
        SubscribeEvents();
        InitializeSpeed();
    }

    private void Update()
    {
        RunInput();
        Movement();
    }
    private void OnDestroy()
    {
        UnSubscribeEvents();
    }

    private void InitializeComponents()
    {
        _Controller = GetComponent<CharacterController>();
        _Player = GetComponent<Player>();
    }

    private void InitializeSpeed()
    {
        _walkSpeed = _Player.PlayerStats.MoveSpeed.Current;
        _currentSpeed = _walkSpeed;
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

    private void RunInput()
    {
        bool shouldRun = InputManager.Instance.GetKey(EGameKeyType.Run);

        if (shouldRun != _isRunning)
        {
            _isRunning = shouldRun;
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
        _currentSpeed = _isRunning? _walkSpeed * _runSpeedMultiplier : _walkSpeed;
    }
}
