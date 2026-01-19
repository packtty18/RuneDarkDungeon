using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyAttack/Sound")]
public class SoundEffectSO : AttackEffectSO
{
    [SerializeField] private ESoundType soundType;
    public override void Execute(EnemyAttackObject owner, Vector3 position)
    {
        if(!SoundManager.IsExist())
        {
            return;
        }

        SoundManager.Instance.Play(soundType, position);
    }
}

