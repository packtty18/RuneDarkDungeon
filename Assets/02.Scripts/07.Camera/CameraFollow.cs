using DG.Tweening;
using TMPro;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Transform Distance;

    private Camera _camera;
    private Vector3 _offset;

    public Vector3 BasePosition { get; private set; }


    private void Start()
    {
        _camera = GetComponent<Camera>();

        _offset = Distance.localPosition;
    }

    private void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 rotatedOffset = Target.rotation * _offset;
            BasePosition = Target.position + rotatedOffset;

            transform.position = BasePosition;
        }
    }
}