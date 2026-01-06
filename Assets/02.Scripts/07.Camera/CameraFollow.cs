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
        transform.rotation = Distance.rotation;
    }

    private void LateUpdate()
    {
        if (Target != null)
        {
            BasePosition = Target.position + _offset;
            
            transform.position = BasePosition;
        }
    }
}