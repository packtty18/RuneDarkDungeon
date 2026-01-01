using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (InputManager.Instance.GetKey(EGameKeyType.Front))
        {
            Debug.Log("앞으로 이동");
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Back))
        {
            Debug.Log("뒤로 이동");
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Left))
        {
            Debug.Log("왼쪽으로 이동");
        }
        if (InputManager.Instance.GetKey(EGameKeyType.Right))
        {
            Debug.Log("오른쪽으로 이동");
        }
    }
}
