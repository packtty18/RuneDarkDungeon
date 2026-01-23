using System;
using System.Collections;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    private ParticleSystem[] _particles;

    public event Action OnPlay;
    public event Action<float> OnAttack;

    [SerializeField]
    private float _playTime = 2;
    void Awake()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
    }

    public void Emit()
    {
        foreach (var particle in _particles)
        {
            particle.Emit(1);
        }
    }

    public void Play()
    {
        foreach (var particle in _particles)
        {
            particle.Play();
        }
        OnPlay?.Invoke();
    }

    public void PlayDealEffectWorldPosition(Transform parent, Vector3 position, float damage)
    {
        gameObject.transform.SetParent(null);
        gameObject.transform.position = position;

        OnAttack?.Invoke(damage);
        Play();
        
        StartCoroutine(ReturnTransform(parent, _playTime));
    }

    public void PlayAt(Transform parent, Transform transform)
    {
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = parent.rotation;

        Play();
        gameObject.transform.SetParent(null);

        StartCoroutine(ReturnTransform(parent, _playTime));
    }

    public bool PlayEnd()
    {
        foreach (var particle in _particles)
        {
           if (particle.isPlaying)
            {
                return false;
            }
        }
        return true;
    }
    public IEnumerator ReturnTransform(Transform parent, float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.transform.SetParent(parent);
        yield return null;
    }
}
