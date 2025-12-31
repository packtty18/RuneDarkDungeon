using UnityEngine;

public class GlobalManagersRoot : MonoBehaviour
{
    private static GlobalManagersRoot _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}