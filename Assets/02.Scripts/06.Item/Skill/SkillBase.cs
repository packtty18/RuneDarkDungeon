using UnityEngine;

public class SkillBase : MonoBehaviour
{
    [Header("데미지 설정")]
    [SerializeField] private float _damage;
    
    [Header("지속 시간 설정")]
    [SerializeField] private float _lifeTime = 2.0f;

    private ParticleSystem[] _particles;

    private void Awake()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void PlayAllParticles()
    {
        if (_particles == null) return;

        foreach (var particle in _particles)
        {
            particle.Clear();
            particle.Play();
        }
    }

    protected virtual void ApplyEffect(GameObject user, EItemGrade grade) { }

    public void OnUse(GameObject user, EItemGrade grade)
    {
        PlayAllParticles();
        ApplyEffect(user, grade);
        Destroy(gameObject, _lifeTime);
    }

    public virtual void OnEquip(GameObject user, EItemGrade grade) { }
    public virtual void OnUnequip(GameObject user, EItemGrade grade) { }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
