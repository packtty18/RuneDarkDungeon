using UnityEngine;

namespace ProceduralMeshExploder
{
    public class Cluster : MonoBehaviour
    {
        public MeshFilter MeshFilter;
        public MeshRenderer MeshRenderer;
        public BoxCollider BoxCollider;
        public MeshCollider MeshCollider;
        public Rigidbody Rigidbody;

        private const float MIN_COLLIDER_SIZE = 0.01f;

        public Cluster Spawn(
            Triangle[] triangleGroup,
            Vector3 velocity,
            int layer,
            bool useMeshColliders,
            bool recalculateNormals)
        {
            Vector3[] vertices = new Vector3[triangleGroup.Length * 6];
            int[] triangles = new int[vertices.Length * 2];

            int vertexIndex = 0;
            int trisIndex = 0;

            for (int i = 0; i < triangleGroup.Length; i++)
            {
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[i].Vertices[0];
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[i].Vertices[1];
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[i].Vertices[2];

                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[i].Vertices[2];
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[i].Vertices[1];
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[i].Vertices[0];
            }

            gameObject.layer = layer;

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            if (recalculateNormals)
                mesh.RecalculateNormals();

            mesh.RecalculateBounds();
            MeshFilter.sharedMesh = mesh;

            Bounds bounds = mesh.bounds;

            bool canUseMeshCollider =
                useMeshColliders &&
                vertices.Length >= 4 &&
                bounds.size.x > MIN_COLLIDER_SIZE &&
                bounds.size.y > MIN_COLLIDER_SIZE &&
                bounds.size.z > MIN_COLLIDER_SIZE;

            MeshCollider.enabled = false;
            BoxCollider.enabled = false;

            if (canUseMeshCollider)
            {
                MeshCollider.sharedMesh = mesh;
                MeshCollider.convex = true; // ⭐ 필수
                MeshCollider.enabled = true;
            }
            else
            {
                BoxCollider.size = bounds.size;
                BoxCollider.center = bounds.center;
                BoxCollider.enabled = true;

                Debug.Log(
                    $"[Cluster] Fallback to BoxCollider (size={bounds.size})",
                    this
                );
            }

            Rigidbody.linearVelocity = velocity;
            Rigidbody.angularVelocity = Random.insideUnitSphere * 360f;

            gameObject.SetActive(true);
            return this;
        }
    }
}
