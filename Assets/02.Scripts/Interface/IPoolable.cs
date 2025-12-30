using UnityEngine;

public interface IPoolable
{
    void Get();
    void Release();
}
