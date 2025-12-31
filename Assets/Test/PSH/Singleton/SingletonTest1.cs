using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonTest1 : MonoBehaviour
{
    [Button]
    public void NextScene()
    {
        SceneManager.LoadScene("Singleton2");
    }
}
