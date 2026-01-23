using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(UI_BasicAnimation))]
[RequireComponent(typeof(CanvasGroup))]
public class UI_Popup : UIBase
{

    [SerializeField]
    private bool _useKey = false;

    [SerializeField, ShowIf(nameof(_useKey))]
    private EGameKeyType _upKey = EGameKeyType.None;
    [SerializeField, ShowIf(nameof(_useKey))] 
    private EGameKeyType _downKey = EGameKeyType.None;

    [SerializeField]
    private bool _useButton = false;

    [SerializeField, ShowIf(nameof(_useButton))]
    private Button _showButton;

    [SerializeField]
    private bool _popUpAnimation = true;

    [SerializeField]
    private bool _popDownAnimation = false;

    [SerializeField]
    private float _popDuration = 0.5f;

    [SerializeField]
    private float _startScale = 0.5f;

    private UI_BasicAnimation _animation;

    private InputManager _inputManager;

    private bool _isOpened = false;


    protected override void Awake()
    {
        base.Awake();
        TryGetComponent<UI_BasicAnimation>(out _animation);
        _showButton?.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (_isOpened)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Start()
    {
        _animation.Hide();
        _inputManager = InputManager.Instance;
    }

    private void Update()
    {
        InputCheck();
    }

    public void InputCheck()
    {
        if (_upKey != EGameKeyType.None)
        {
            if (!_isOpened && _inputManager.GetKeyDown(_upKey))
            {
                Show();
                return;
            }
        }
        if (_downKey != EGameKeyType.None)
        {
            if (_isOpened && _inputManager.GetKeyDown(_downKey))
            {
                Hide();
                return;
            }
        }
    }

    public override void Show()
    {
        if (SceneLoadManager.IsExist() && SceneLoadManager.Instance.CurrentSceneData.IsCursorLocked)
        {
            CursorManager.Instance?.SetCursorLock(false);
        }

        if (_popUpAnimation)
        {
            _animation.PopUp(_popDuration, _startScale);
        }
        else
        {
            _animation?.Show();
        }
        _isOpened = true;
    }

    public override void Hide()
    {
        if (SceneLoadManager.IsExist() && SceneLoadManager.Instance.CurrentSceneData.IsCursorLocked)
        {
            CursorManager.Instance?.SetCursorLock(true);
        }

        if (_popDownAnimation)
        {
            _animation.PopDown(_popDuration, _startScale);
        }else
        {
            _animation.Hide();
        }
        _isOpened = false;
    }

    public void OnDestroy()
    {
        _showButton?.onClick.RemoveAllListeners();
    }
}
