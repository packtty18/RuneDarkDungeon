using UnityEngine;

public class CreateBox : MonoBehaviour
{
    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BoxFactory.Instance.Create();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.nearClipPlane));
            Vector3 dir = (mousePos - _camera.transform.position).normalized;

            RaycastHit hit;
            Ray ray = new Ray(_camera.transform.position, dir);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Util.ObjectDestroy(hit.collider.gameObject);
            }
        }
    }
    

}
