using UnityEngine;

[RequireComponent(typeof(SceneTransition))]
public class KeySceneTransitionUI : MonoBehaviour
{
    
    private SceneTransition _transition;
    private UIBasicAnimation _animation;

    [SerializeField]
    private EGameKeyType _gameKey;

    [SerializeField]
    private bool _fadeLoop = false;
    [SerializeField]
    private float _fadedTime = 0.3f;
    [SerializeField]
    private float _fadedOutValue = 0.3f;

    private void Awake()
    {
        _transition = GetComponent<SceneTransition>();
        if (_fadeLoop)
        {
            TryGetComponent<UIBasicAnimation>(out _animation);
        }
    }
    private void Start()
    {
        if (_fadeLoop && _animation != null)
        {
            _animation.FadeLoop(_fadedTime, _fadedOutValue);
        }

        if (_transition == null)
        {
            Debug.LogError("SceneTransition 컴포넌트를 찾을 수 없습니다.");
        }

    }
    void Update()
    {
        if (InputManager.Instance.GetKeyDown(_gameKey))
        {
            _transition.TransitionToScene();
        }
    }
}
