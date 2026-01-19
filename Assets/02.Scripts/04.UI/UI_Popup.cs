using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UI_BasicAnimation))]
[RequireComponent(typeof(CanvasGroup))]
public class UI_Popup : UIBase
{
    [SerializeField]
    private EGameKeyType _upKey;
    [SerializeField] 
    private EGameKeyType _downKey;

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
        Hide();
    }

    private void Start()
    {
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
        if (SceneLoadManager.Instance.CurrentSceneData.IsCursorLocked)
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
        if (SceneLoadManager.Instance.CurrentSceneData.IsCursorLocked)
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
}
