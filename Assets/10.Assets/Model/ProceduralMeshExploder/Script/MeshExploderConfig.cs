using UnityEngine;

namespace ProceduralMeshExploder 
{
    [CreateAssetMenu(fileName = "New Mesh Exploder Config", menuName = "Mesh Exploder/New Mesh Exploder Config")]
    public class MeshExploderConfig : ScriptableObject
    {
        [Tooltip("Amount of clusters")]
        [Min(2)]
        public int Clusters;

        [Tooltip("Lifetime of clusters in seconds")]
        [Min(0.01f)]
        public float Lifetime;

        [Tooltip("Explosion power of clusters")]
        [Min(0.01f)]
        public float Power;

        [Tooltip("Constant additional force applied to the clusters")]
        public Vector3 AdditionalForce;

        [Tooltip("Additional force multiplier over lifetime")]
        public AnimationCurve ForceMultiplier;

        [Tooltip("Gameobject layer index for spawned Clusters")]
        public int ClustersLayer;

        [Tooltip("Always use mesh colliders for clusters (Default is Box Colliders)")]
        public bool UseMeshColliders;

        [Tooltip("Recalculate normals for clusters")]
        public bool RecalculateNormals;

        [Tooltip("Color animations over lifetime")]
        public ColorAnimation[] ColorAnimations;
    }
}