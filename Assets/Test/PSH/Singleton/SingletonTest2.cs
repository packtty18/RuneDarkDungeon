using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonTest2 : MonoBehaviour
{
    [Button]
    public void NextScene()
    {
        SceneManager.LoadScene("Singleton1");
    }
}
