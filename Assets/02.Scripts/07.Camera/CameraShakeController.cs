using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineBasicMultiChannelPerlin))]
public class CameraShakeController : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin _perlinNoise;
    private Coroutine _coroutine;


    private void Awake()
    {
        _perlinNoise = GetComponent<CinemachineBasicMultiChannelPerlin>();
        _perlinNoise.AmplitudeGain = 0;
    }

    public void CameraShake(float intensity, float duration)
    {
        _perlinNoise.AmplitudeGain = intensity;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine );
        }
        _coroutine = StartCoroutine(ShakeCoroutine(duration));
    }

    public void CameraShake(float intensity)
    {
        _perlinNoise.AmplitudeGain = intensity;
    }

    public IEnumerator ShakeCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopCameraShake();
    }

    public void StopCameraShake()
    {
        _perlinNoise.AmplitudeGain = 0;
    }


}
