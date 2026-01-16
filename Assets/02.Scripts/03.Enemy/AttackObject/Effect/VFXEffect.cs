using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyAttack/VFX")]
public class VFXEffectSO : AttackEffectSO
{
    public override void Execute(EnemyAttackObject owner, Vector3 position)
    {
        ParticleSystem[] particles = owner.gameObject.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem particle in particles)
        {
            particle.Clear();
            particle.Play();
        }
    }
}
