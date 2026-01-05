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

        public Cluster Spawn(Triangle[] triangleGroup, Vector3 velocity, int layer, bool useMeshColliders, bool recalculateNormals)
        {
            //Debug.DrawLine(transform.position, transform.position + velocity, Color.green, 30.0f, false);

            Vector3[] vertices;
        
            int[] triangles;

            vertices = new Vector3[triangleGroup.Length * 6];
            triangles = new int[vertices.Length * 2];

            int vertexIndex = 0;
            int trisIndex = 0;

            for (int triangle = 0; triangle < triangleGroup.Length; triangle++)
            {
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[triangle].Vertices[0];
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[triangle].Vertices[1];
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[triangle].Vertices[2];
            
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[triangle].Vertices[2];
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[triangle].Vertices[1];
                triangles[trisIndex++] = vertexIndex;
                vertices[vertexIndex++] = triangleGroup[triangle].Vertices[0];
            }

            gameObject.layer = layer;

            MeshFilter.sharedMesh = new Mesh();
            MeshFilter.sharedMesh.vertices = vertices;
            MeshFilter.sharedMesh.triangles = triangles;
            if(recalculateNormals) MeshFilter.sharedMesh.RecalculateNormals();

            if (useMeshColliders)
            {
                MeshCollider.sharedMesh = MeshFilter.sharedMesh;
                MeshCollider.enabled = true;
            }
            else
            {
                Bounds bounds = GeometryUtility.CalculateBounds(vertices, Matrix4x4.identity);
                BoxCollider.size = bounds.size;
                BoxCollider.center = bounds.center;
                BoxCollider.enabled = true;
            }

            Rigidbody.linearVelocity = velocity;
            Rigidbody.angularVelocity = Random.rotation.eulerAngles * 360.0f;

            gameObject.SetActive(true);
            return this;
        }
    }
}