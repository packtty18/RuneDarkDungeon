using Unity.VisualScripting;
using UnityEngine;

public class CreateBox : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BoxFactory.Instance.Create();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Vector3 dir = (mousePos - Camera.main.transform.position).normalized;

            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, dir);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Util.ObjectDestroy(hit.collider.gameObject);
            }
        }
    }
    

}
