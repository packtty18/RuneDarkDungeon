using UnityEngine;

public class GlobalManagersRoot : MonoBehaviour
{
    private static GlobalManagersRoot s_instance;

    void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }
}