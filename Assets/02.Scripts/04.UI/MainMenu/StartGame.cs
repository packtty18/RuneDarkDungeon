using UnityEngine;

public class StartGame : MonoBehaviour
{
    
    private SceneTransition _transition;

    private void Start()
    {
        _transition = GetComponent<SceneTransition>();
        if (_transition == null)
        {
            Debug.LogError("SceneTransition 컴포넌트를 찾을 수 없습니다.");
        }
    }
    void Update()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Enter))
        {
            _transition.TransitionToScene();
        }
    }
}
