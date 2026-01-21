using Sirenix.OdinInspector;
using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Meteor : PoolableObject
{
    [Header("충돌 설정")]
    [SerializeField] private EPoolType _meteorHit;
    [SerializeField] private LayerMask _hitLayer;
    [SerializeField] private RangeData<float> _damage;

    [Header("소리")]
    [SerializeField]
    private bool _sound;
    [SerializeField, EnableIf(nameof(_sound))]
    private ESoundType[] _sounds;

    [Header("카메라 쉐이크")]
    [SerializeField]
    private bool _cameraShake;
    [SerializeField, EnableIf(nameof(_cameraShake))]
    private float _intensity = 1f;
    [SerializeField, EnableIf(nameof(_cameraShake))]
    private float _duration = 1f;

    public Transform m_hitObject;
    public float maxLength;
    public bool isDestroy;
    public float ObjectDestroyTime;
    public float maxTime = 1;
    public float MoveSpeed = 10;
    public bool isHitMake = true;

    float time;
    bool ishit;

    public override void OnSpawn()
    {
        base.OnSpawn();
        transform.position = Vector3.zero;
        time = Time.time;
        ishit = false;
    }
    
    void LateUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
        if (!ishit)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxLength, _hitLayer))
                HitObj(hit);
        }

        if (isDestroy)
        {
            if (Time.time > time + ObjectDestroyTime)
            {
                MakeHitObject(transform);
                ReturnToPool();
            }
        }
    }

    void MakeHitObject(RaycastHit hit)
    {
        if (isHitMake == false)
            return;
        
        var m_makedObject = PoolManager.Instance.Get(_meteorHit);
        
        m_makedObject.transform.position = hit.point;
        m_makedObject.transform.rotation = Quaternion.LookRotation(hit.normal);

        if (!m_makedObject.TryGetComponent(out ExplosionSkill explosion)) return;
        explosion.Explosion(_damage.GetRandomValue());
    }

    void MakeHitObject(Transform point)
    {
        if (isHitMake == false)
            return;
        
        var m_makedObject = PoolManager.Instance.Get(_meteorHit);
        
        m_makedObject.transform.position = point.position;
        m_makedObject.transform.rotation = point.rotation;
        
        if (!m_makedObject.TryGetComponent(out ExplosionSkill explosion)) return;
        explosion.Explosion(_damage.GetRandomValue());
    }

    void HitObj(RaycastHit hit)
    {
        ishit = true;
        MakeHitObject(hit);
        
        if (_cameraShake)
        {
            HitShake();
        }
        if (_sound)
        {
            SoundManager.Instance.Play(_sounds[Random.Range(0, _sounds.Length - 1)], transform.position);
        }

        ReturnToPool();
    }

    void HitShake()
    {
        CameraManager.Instance?.CameraShake(_intensity, _duration);
    }
}
