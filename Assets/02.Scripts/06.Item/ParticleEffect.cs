using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [Header("지속 시간 설정")]
    [SerializeField] private float _lifeTime = 2.0f;

    private ParticleSystem[] _particles;

    private void Awake()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        PlayAllParticles();
        Destroy(gameObject, _lifeTime);
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

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
