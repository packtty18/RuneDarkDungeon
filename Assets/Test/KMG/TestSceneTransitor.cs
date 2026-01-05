using UnityEngine;

public class TestSceneTransitor : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SceneTransition>().TransitionToScene();
    }
}
