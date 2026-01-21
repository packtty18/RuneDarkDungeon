using UnityEngine;

public abstract class SkillBase : PoolableObject
{
    [Header("데미지 설정")]
    [SerializeField] protected SerializableDictionary<EItemGrade, RangeData<float>> _damage;
    
    [Header("지속 시간 설정")]
    [SerializeField] protected float _lifeTime = 2.0f;

    private ParticleSystem[] _particles;
    protected GameObject _user;
    protected EItemGrade _grade;

    private void Awake()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
		OnInit();
    }
    
    protected virtual void OnInit() { }

    private void PlayAllParticles()
    {
        if (_particles == null) return;

        foreach (var particle in _particles)
        {
            particle.Clear();
            particle.Play();
        }
    }

    public void OnUse(GameObject user, EItemGrade grade)
    {
        _user = user;
        _grade = grade;
        
        PlayAllParticles();
        ApplyEffect(_user, _grade);
        ReturnToPoolAfter(_lifeTime);
    }

    protected abstract void ApplyEffect(GameObject user, EItemGrade grade);
}
