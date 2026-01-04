using UnityEngine;

public class BoxFactory : GlobalSingleton<BoxFactory>
{
    [SerializeField]
    private string key = "Box";

    public void Start()
    {
        if (!PoolManager.Instance.HasPool(key))
        {
            throw new System.Exception($"BoxFactory Start Error: Pool with key '{key}' does not exist.");
        }
    }
    public void Create()
    { 
        PoolManager.Instance.Get<Box>(key);
    }

    public void CreateAt(Vector3 position)
    {
        var box = PoolManager.Instance.Get<Box>(key);
        box.transform.position = position;
    }
}
