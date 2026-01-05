using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralMeshExploder 
{
    public class MeshExploder : MonoBehaviour
    {
        public MeshExploderConfig Config;
        public Cluster ClusterPrefab;

        Renderer parentRenderer;
        bool animating;

        public void Explode()
        {
            gameObject.SetActive(true);
            if (!animating)
            {
                animating = true;
                StartCoroutine(Animate());
            }
        }

        IEnumerator Animate()
        {
            Mesh defaultMesh;

            if (transform.parent)
            {
                if (transform.parent.TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
                {
                    if (skinnedMeshRenderer.sharedMesh)
                    {
                        parentRenderer = skinnedMeshRenderer;
                        defaultMesh = new Mesh();
                        skinnedMeshRenderer.BakeMesh(defaultMesh);
                        parentRenderer.enabled = false;
                    }
                    else
                    {
                        Debug.LogWarning(messageParentMeshNotFound);
                        yield break;
                    }
                }
                else if (transform.parent.TryGetComponent(out MeshRenderer meshRenderer))
                {
                    parentRenderer = meshRenderer;

                    if (transform.parent.TryGetComponent(out MeshFilter parentMeshFilter))
                    {
                        if (parentMeshFilter.sharedMesh)
                        {
                            defaultMesh = parentMeshFilter.mesh;
                            meshRenderer.enabled = false;
                        }
                        else
                        {
                            Debug.LogWarning(messageParentMeshNotFound);
                            yield break;
                        }
                    }
                    else
                    {
                        Debug.LogWarning(messageParentMeshNotFound);
                        yield break;
                    }
                }
                else
                {
                    Debug.LogWarning(messageParentRendererNotFound);
                    yield break;
                }
            }
            else
            {
                Debug.LogWarning(messageParentNotFound);
                yield break;
            }

            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localEulerAngles = Vector3.zero;
            transform.parent = null;

            Vector3[] importedVertices = defaultMesh.vertices;
            int[] importedTriangles = defaultMesh.triangles;

            Vector3[] flatVertices = new Vector3[importedTriangles.Length];
            int[] flatTriangles = new int[importedTriangles.Length];

            for (int i = 0; i < importedTriangles.Length; i++)
            {
                flatVertices[i] = importedVertices[importedTriangles[i]];
                flatTriangles[i] = i;
            }

            ////////////////////////// Start Animation

            Triangle[] triangles = new Triangle[importedTriangles.Length / 3];

            for (int vertexIndex = 0; vertexIndex < importedTriangles.Length; vertexIndex += 3)
            {
                int triangleIndex = vertexIndex / 3;

                triangles[triangleIndex] = new Triangle()
                {
                    Vertices = new Vector3[3] { 
                        flatVertices[flatTriangles[vertexIndex]],
                        flatVertices[flatTriangles[vertexIndex + 1]],
                        flatVertices[flatTriangles[vertexIndex + 2]] 
                    }
                };
            }

            Bounds importedBounds = defaultMesh.bounds;

            float vertexRadius = Vector3.Distance(importedBounds.min, importedBounds.max);

            //Debug.DrawLine(importedBounds.min, importedBounds.max, Color.red, 30.0f, false);

            Vector3[] clusterPoints = new Vector3[Config.Clusters];

            for (int i = 0; i < Config.Clusters; i++)
            {
                clusterPoints[i] = vertexRadius * Random.insideUnitSphere;
                //Debug.DrawLine(transform.position, transform.position + clusterPoints[i], UnityEngine.Color.red, Config.Lifetime, false);
            }

            List<Triangle>[] triangleGroups = new List<Triangle>[Config.Clusters];

            for (int i = 0; i < triangleGroups.Length; i++) triangleGroups[i] = new List<Triangle>();

            List<Triangle> nonSeperatedTriangles = new List<Triangle>(triangles);
            Vector3 point;
            Triangle triangle;

            while (nonSeperatedTriangles.Count > 0)
            {
                float minDistance = Mathf.Infinity;
                int closestPoint = -1;
                triangle = nonSeperatedTriangles.First();

                for (int i = 0; i < clusterPoints.Length; i++)
                {
                    point = clusterPoints[i].normalized;

                
                    float distance = (point - triangle.Position).sqrMagnitude;
                
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestPoint = i;
                    }
                }

                nonSeperatedTriangles.RemoveAt(0);
                triangleGroups[closestPoint].Add(triangle);
            }

            List<Cluster> clusters = new List<Cluster>();

            for (int i = 0; i < Config.Clusters; i++)
            {
                if (triangleGroups[i].Count == 0) continue;
                clusters.Add(GetNewCluster(triangleGroups[i].ToArray(), clusterPoints[i].normalized));
            }

            Cluster[] clustersArray = clusters.ToArray();

            MaterialPropertyBlock matPropBlock = new MaterialPropertyBlock();
            ClusterPrefab.MeshRenderer.GetPropertyBlock(matPropBlock);


            ConstantForce[] forces = new ConstantForce[0];

            bool useForce = Config.AdditionalForce != Vector3.zero;

            if (useForce)
            {
                forces = new ConstantForce[clustersArray.Length];

                for (int i = 0; i < clustersArray.Length; i++)
                {
                    forces[i] = clustersArray[i].gameObject.AddComponent<ConstantForce>();
                }
            }

            float alpha = 0.0f;

            while (animating)
            {
                alpha = Mathf.Min(1.0f, alpha + Time.deltaTime / Config.Lifetime);

                for (int i = 0; i < Config.ColorAnimations.Length; i++)
                {
                    matPropBlock.SetColor(Config.ColorAnimations[i].MaterialProperty, Config.ColorAnimations[i].ColorOverLife.Evaluate(alpha));
                }

                for (int i = 0; i < clustersArray.Length; i++)
                {
                    clustersArray[i].MeshRenderer.SetPropertyBlock(matPropBlock);
                    if(useForce) forces[i].force = Config.ForceMultiplier.Evaluate(alpha) * Config.AdditionalForce;
                }
            
                yield return new WaitForEndOfFrame();

                if (alpha == 1.0f)
                {
                    animating = false;
                }
            }

            for (int i = 0; i < clustersArray.Length; i++)
            {
                Destroy(clustersArray[i].gameObject);
                yield return new WaitForEndOfFrame();
            }

            Destroy(gameObject);
        }

        public Cluster GetNewCluster(Triangle[] triangleGroup, Vector3 direction)
        {
            Cluster cluster = Instantiate(ClusterPrefab, transform.position, transform.rotation);
            cluster.transform.localScale = transform.lossyScale;
            return cluster.Spawn(triangleGroup, direction * Config.Power, Config.ClustersLayer, Config.UseMeshColliders, Config.RecalculateNormals);
        }

        const string messageParentMeshNotFound = "MeshExploder: Parent Mesh not found.";
        const string messageParentRendererNotFound = "MeshExploder: MeshRenderer/SkinnedMeshRenderer not found on parent object. Please add MeshExploder as a child object into targeted object's transform hierarchy.";
        const string messageParentNotFound = "MeshExploder: Parent object not found. Please add MeshExploder as a child object into targeted object's transform hierarchy.";
    }

    public struct Triangle
    {
        public Vector3[] Vertices;
        public Vector3 Position { get { return (Vertices[0] + Vertices[1] + Vertices[2]) / 3; } }
    }
}