using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : GlobalSingleton<InputManager>
{
    // 키에 대응하는 kecode
    private Dictionary<EGameKeyType, KeyCode[]> _keyMapping = new Dictionary<EGameKeyType, KeyCode[]>()
    {
        { EGameKeyType.Front,      new KeyCode[]{ KeyCode.UpArrow, KeyCode.W } },
        { EGameKeyType.Back,    new KeyCode[]{ KeyCode.DownArrow, KeyCode.S } },
        { EGameKeyType.Left,    new KeyCode[]{ KeyCode.LeftArrow, KeyCode.A, KeyCode.Z } },
        { EGameKeyType.Right,   new KeyCode[]{ KeyCode.RightArrow, KeyCode.D, KeyCode.Slash } },
        { EGameKeyType.Jump,    new KeyCode[]{ KeyCode.Space } },
        { EGameKeyType.Run,    new KeyCode[]{ KeyCode.LeftShift } },
        { EGameKeyType.Attack,  new KeyCode[]{ KeyCode.Mouse0 } },
        { EGameKeyType.QSkill,   new KeyCode[]{ KeyCode.Q} },
        { EGameKeyType.ESkill,   new KeyCode[]{ KeyCode.E} },
        { EGameKeyType.RSkill,   new KeyCode[]{ KeyCode.R} },
        { EGameKeyType.Enter,   new KeyCode[]{ KeyCode.Return} }
    };

    // 키 상태 저장
    private Dictionary<EGameKeyType, bool> _currentDownStates = new Dictionary<EGameKeyType, bool>();
    private Dictionary<EGameKeyType, bool> _previousDownStates = new Dictionary<EGameKeyType, bool>();
    private EGameKeyType[] _gameKeyType;


    protected override void Awake()
    {
        base.Awake();
        _gameKeyType = (EGameKeyType[])Enum.GetValues(typeof(EGameKeyType));

        _currentDownStates = _gameKeyType.ToDictionary(key => key, _ => false);
        _previousDownStates = _gameKeyType.ToDictionary(key => key, _ => false);
    }

    void Update()
    {
        foreach (var key in _gameKeyType)
        {
            // 이전 상태 갱신.
            _previousDownStates[key] = _currentDownStates[key];


            // 현재 상태 갱신.
            _currentDownStates[key] = false;
            KeyCode[] codes = _keyMapping[key];

            foreach (var code in codes)
            {
                if (Input.GetKey(code))
                {
                    _currentDownStates[key] = true;
                    break;
                }
            }
        }
    }
    public bool GetKeyDown(EGameKeyType key)
    {
        if (SceneLoadManager.Instance.SceneType == ESceneType.Loading)
        {
            return false;
        }

        //키가 현재 눌렸지만 이전 프레임에 눌리지 않았을 경우.
        return _currentDownStates[key] && !_previousDownStates[key];
    }

    //홀드.
    public bool GetKey(EGameKeyType key)
    {
        //키가 현재 눌려지는 경우.
        return _currentDownStates[key];
    }

    //릴리즈.
    public bool GetKeyUp(EGameKeyType key)
    {
        //키가 현재 눌려지지 않았으나 이전 프레임에 눌려져 있었을 경우.
        return !_currentDownStates[key] && _previousDownStates[key];
    }
}
