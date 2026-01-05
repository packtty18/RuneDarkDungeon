using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController _Controller;
    private Player _Player;

    private float _speed;
    private float _walkSpeed;
    private float _runSpeedMultiplier = 2f;
    private bool _isRunning = false;

    private Action<float> _onMoveSpeedChanged;
    void Start()
    {
        _Controller = GetComponent<CharacterController>();
        _Player = GetComponent<Player>();

        _onMoveSpeedChanged = HandleMoveSpeedChanged;
        _Player.PlayerStats.MoveSpeed.Subscribe(_onMoveSpeedChanged);
        _walkSpeed = _Player.PlayerStats.MoveSpeed.Current;
        _speed = _walkSpeed;
    }

    private void HandleMoveSpeedChanged(float obj)
    {
        _walkSpeed = obj;
        if (_isRunning)
        {
            _speed = _walkSpeed * _runSpeedMultiplier;
        }
        else
        {
            _speed = _walkSpeed;
        }
    }

    void Update()
    {
        if (InputManager.Instance.GetKey(EGameKeyType.Front))
        {
            _Controller.Move(Vector3.forward * Time.deltaTime * _speed);
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Back))
        {
            _Controller.Move(Vector3.back * Time.deltaTime * _speed);
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Left))
        {
            _Controller.Move(Vector3.left * Time.deltaTime * _speed);
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Right))
        {
            _Controller.Move(Vector3.right * Time.deltaTime * _speed);
        }
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Run))
        {
            _speed = _walkSpeed * 2;
            _isRunning = true;
        }
        else if (InputManager.Instance.GetKeyUp(EGameKeyType.Run))
        {
            _speed = _walkSpeed;
            _isRunning = false;
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Attack))
        {
            Debug.Log("공격");
        }
    }
}
