using UnityEngine;
using UnityEngine.UIElements;

public class SoundFactory : PoolFactory<SoundObject>
{
    public SoundFactory(PoolManager poolManager, EPoolType type) : base(poolManager, type)
    {
        if (!_poolManager.HasPool(type))
        {
            throw new System.InvalidOperationException($"SoundFactory Start Error: Pool with key '{type}' does not exist.");
        }
    }

    public SoundObject Create()
    {
        return CreateInternal();
    }

    public SoundObject CreateAt(Vector3 position)
    {
        SoundObject obj = Create();
        obj.SetPosition(position);
        return obj;
    }

    protected override void OnCreated(SoundObject obj) 
    { 
        //사운드 오브젝트의 설정
    }

    public void Play(SoundData data, Vector3 pos)
    {
        SoundObject obj = CreateAt(pos);
        obj.Play(data);
    }
}
