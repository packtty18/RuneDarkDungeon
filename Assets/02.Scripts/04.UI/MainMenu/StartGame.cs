using GameCore.Data;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private SceneDataSO _nextScene;

    void Update()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Enter))
        {
            _nextScene.LoadScene();
        }
    }
}
