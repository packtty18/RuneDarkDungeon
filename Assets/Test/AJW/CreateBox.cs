using UnityEngine;

public class CreateBox : MonoBehaviour
{
    private Camera _camera;
    private BoxFactory _boxFactory;
    private void Start()
    {
        _boxFactory = new BoxFactory(PoolManager.Instance);
        _camera = Camera.main;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _boxFactory.Create();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f))
            {
                Util.ObjectDestroy(hit.collider.gameObject);
            }
        }
    }
}
