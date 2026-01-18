using UnityEngine;

[RequireComponent(typeof(UIBasicAnimation))]
public class PopUpInputController : MonoBehaviour
{
    [SerializeField]
    private EGameKeyType _upKey;
    [SerializeField] 
    private EGameKeyType _downKey;
    [SerializeField]
    private float _popDuration = 0.5f;

    private UIBasicAnimation _animation;

    private InputManager _inputManager;


    void Awake()
    {
        TryGetComponent<UIBasicAnimation>(out _animation);
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
                PopUp();
            }
        }
        if (_downKey != EGameKeyType.None)
        {
            if (_inputManager.GetKeyDown(_downKey))
            {
                PopDown();
            }
        }
    }

    public void PopUp()
    {
        _animation.PopUp(_popDuration);
    }

    public void PopDown()
    {
        _animation.PopDown(_popDuration);
    }
}
