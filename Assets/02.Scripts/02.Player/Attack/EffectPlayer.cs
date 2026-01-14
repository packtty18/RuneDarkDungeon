using System.Collections;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    private ParticleSystem[] _particles;
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
    }

    public void PlayAt(Transform parent, Transform transform)
    {
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = parent.rotation;

        Play();
        gameObject.transform.SetParent(null);

        StartCoroutine(ReturnTransform(parent));
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

    public IEnumerator ReturnTransform(Transform parent)
    {
        while (!PlayEnd())
        {
            yield return null;
        }

        gameObject.transform.SetParent(parent);
        yield return null;
    }
}
