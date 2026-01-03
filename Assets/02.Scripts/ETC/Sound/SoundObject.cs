using System;
using System.Collections;
using UnityEngine;

//추후 풀링 기능 추가할것.
[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour
{
    private AudioSource _audio;
    private Coroutine _lifeRoutine;

    public SafeEvent OnFinished = new SafeEvent();

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    //아직 풀링 기능 없음
    //스폰되거나 디스폰된다면 재생 루틴 정지
    //오직 Play메서드를 통해서만 재생 가능
    //public void OnSpawn()
    //{
    //    StopRoutine();
    //}

    //public void OnDespawn()
    //{
    //    StopRoutine();
    //}

    public void Play(SoundData data, Vector3 position)
    {
        transform.position = position;

        _audio.clip = data.clip;
        _audio.volume = data.volume;
        _audio.spatialBlend = data.is3D ? 1f : 0f;
        _audio.minDistance = data.minDistance;
        _audio.maxDistance = data.maxDistance;
        _audio.loop = data.isBgm == true;

        _audio.Play();

        if (data.isBgm == false)
        {
            _lifeRoutine = StartCoroutine(LifeRoutine(data.clip.length));
        }
    }

    public void Stop()
    {
        _audio.Stop();
        StopRoutine();
    }


    private IEnumerator LifeRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        OnFinished?.Invoke();
    }

    private void StopRoutine()
    {
        if (_lifeRoutine != null)
        {
            StopCoroutine(_lifeRoutine);
            _lifeRoutine = null;
        }
    }
}
