using UnityEngine;

public class SoundFactory : PoolFactory<SoundObject>
{
    public SoundFactory(PoolManager poolManager, string key) : base(poolManager, key)
    {
        if (!_poolManager.HasPool(_key))
        {
            throw new System.InvalidOperationException($"SoundFactory Start Error: Pool with key '{_key}' does not exist.");
        }
    }

    public SoundObject Create()
    {
        return CreateInternal();
    }

    public SoundObject CreateAt(Vector3 position)
    {
        var obj = CreateInternal();
        obj.transform.position = position;
        return obj;
    }

    protected override void OnCreated(SoundObject obj) 
    { 
        //사운드 오브젝트의 설정
    }
}
