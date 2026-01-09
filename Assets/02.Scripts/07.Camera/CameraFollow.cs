using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Transform _offset;

    public Vector3 BasePosition { get; private set; }


    private void Start()
    {
        transform.rotation = _offset.rotation;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            BasePosition = _target.position + _offset.transform.localPosition;
            
            transform.position = BasePosition;
        }
    }
}