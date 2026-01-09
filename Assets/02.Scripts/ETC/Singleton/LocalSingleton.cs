using UnityEngine;

public abstract class LocalSingleton<T> : SingletonBase<T> where T : MonoBehaviour
{
    protected override bool ShouldDestroyOnLoad => false;
}