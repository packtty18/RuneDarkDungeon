using UnityEngine;

[RequireComponent(typeof(UI_BasicAnimation))]
public class UI_DefaultAnimation : MonoBehaviour
{
    [SerializeField]
    private EAnimationType _defaultAnimation;
    [SerializeField]
    private float _duration = 1.0f;
    [SerializeField]
    private float _minScale = 0f;
    [SerializeField]
    private float _minAlpha = 0f;

    private UI_BasicAnimation _animation;

    private void Awake()
    {
        TryGetComponent<UI_BasicAnimation>(out _animation);
    }
    void Start()
    {
        switch (_defaultAnimation)
        {
            case EAnimationType.AlphaLoop:
                _animation.FadeLoop(_duration, _minAlpha);
                break;
            case EAnimationType.ScaleLoop:
                _animation.ScaleLoop(_duration, _minScale);
                break;
            case EAnimationType.AlphaAndScaleLoop:
                _animation.ScaleAndAlphaLoop(_duration, _minScale, _minAlpha);
                break;
        }
    }
}
