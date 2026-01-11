using UnityEngine;

[ExecuteAlways]
public class TransformFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [Header("Offset")]
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private Vector3 _rotationOffsetEuler;

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        Quaternion rotationOffset = Quaternion.Euler(_rotationOffsetEuler);

        Vector3 finalPosition = _target.TransformPoint(_positionOffset);
        Quaternion finalRotation = _target.rotation * rotationOffset;

        transform.SetPositionAndRotation(
            finalPosition,
            finalRotation
        );
    }
}

