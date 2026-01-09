using UnityEngine;

public class ClickToExplode : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponentInChildren<ProceduralMeshExploder.MeshExploder>().Explode();
        Destroy(gameObject);
    }
}
