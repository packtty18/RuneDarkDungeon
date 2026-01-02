using UnityEngine;

public class StartGame : MonoBehaviour
{
    
    private SceneTransition transition;

    private void Start()
    {
        transition = GetComponent<SceneTransition>();
    }
    void Update()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Enter))
        {
            transition.TransitionToScene();
        }
    }
}
