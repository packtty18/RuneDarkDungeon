using UnityEngine;

public class FinalAttack : PoolSpawner
{
    [Header("추가타 설정")]
    [SerializeField] private EPoolType _effect;

    public void Spawn(float damage)
    {
        var effect = GetFromPool(_effect);
        effect.transform.position = transform.position;

        if (!effect.TryGetComponent(out ExplosionSkill explosion)) return;
        explosion.Explosion(damage);
    }
}
