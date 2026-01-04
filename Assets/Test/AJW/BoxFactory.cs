using UnityEngine;

public class BoxFactory : GlobalSingleton<BoxFactory>
{
    [SerializeField]
    private string _key = "Box";

    private void Start()
    {
        if (!PoolManager.Instance.HasPool(_key))
        {
            throw new System.InvalidOperationException($"BoxFactory Start Error: Pool with key '{_key}' does not exist.");
        }
    }
    public void Create()
    { 
        PoolManager.Instance.Get<Box>(_key);
    }

    public void CreateAt(Vector3 position)
    {
        var box = PoolManager.Instance.Get<Box>(_key);
        box.transform.position = position;
    }
}
