using UnityEngine;

public class CreateBox : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private BoxFactory _boxFactory;
    private void Start()
    {
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
