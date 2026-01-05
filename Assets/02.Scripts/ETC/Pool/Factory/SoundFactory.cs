using UnityEngine;
using UnityEngine.UIElements;

public class SoundFactory : PoolFactory
{
    public SoundFactory(PoolManager poolManager, EPoolType type) : base(poolManager, type)
    {
        if (!_poolManager.HasPool(type))
        {
            throw new System.InvalidOperationException($"SoundFactory Start Error: Pool with key '{type}' does not exist.");
        }
    }

    public GameObject Create()
    {
        return CreateInternal();
    }

    public GameObject CreateAt(Vector3 position)
    {
        GameObject obj = CreateInternal();
        obj.transform.position = position;
        return obj;
    }

    protected override void OnCreated(GameObject obj) 
    { 
        //사운드 오브젝트의 설정
    }

    public void Play(SoundData data, Vector3 pos)
    {
        GameObject obj = CreateAt(pos);
        SoundObject sound = obj.GetComponent<SoundObject>();
        sound.Play(data);
    }
}
