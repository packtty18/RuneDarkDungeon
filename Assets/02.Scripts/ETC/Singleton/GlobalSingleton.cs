using UnityEngine;

public abstract class GlobalSingleton<T> : SingletonBase<T> where T : MonoBehaviour
{
    protected override bool ShouldDestroyOnLoad => true;
}
