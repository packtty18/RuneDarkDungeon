using UnityEngine;

public class GlobalManagersRoot : MonoBehaviour
{
    private static GlobalManagersRoot instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}