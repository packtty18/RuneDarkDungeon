using UnityEngine;

[RequireComponent(typeof(UI_BasicAnimation))]
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


    protected override void Awake()
    {
        base.Awake();
        TryGetComponent<UI_BasicAnimation>(out _animation);
    }

    private void Start()
    {
        _inputManager = InputManager.Instance;
        _animation.Hide();
    }

    private void Update()
    {
        if (_upKey != EGameKeyType.None)
        {
            if (_inputManager.GetKeyDown(_upKey))
            {
                Show();
            }
        }
        if (_downKey != EGameKeyType.None)
        {
            if (_inputManager.GetKeyDown(_downKey))
            {
                Hide();
            }
        }
    }

    public override void Show()
    {
        if (_popUpAnimation)
        {
            _animation.PopUp(_popDuration, _startScale);
        }
        else
        {
            _animation?.Show();
        }
        
    }

    public override void Hide()
    {
        if (_popDownAnimation)
        {
            _animation.PopDown(_popDuration, _startScale);
        }else
        {
            _animation.Hide();
        }
        
    }
}
