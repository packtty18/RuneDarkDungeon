using UnityEngine;

public class GlobalManagerRoot : MonoBehaviour
{
    private static GlobalManagerRoot s_instance;

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