using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField]
    private Camera _playerCamera;

    private void Update()
    {
        Vector3 rotationDirection = new Vector3(0, _playerCamera.transform.eulerAngles.y, 0f);

        transform.eulerAngles = rotationDirection;
    }
}