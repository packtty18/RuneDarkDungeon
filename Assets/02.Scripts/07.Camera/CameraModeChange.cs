using UnityEngine;

public class CameraModeChange : MonoBehaviour
{
    [SerializeField]
    ECameraMode _targetCamera;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager.Instance.SetCameraMode(_targetCamera);
        }
    }
}
