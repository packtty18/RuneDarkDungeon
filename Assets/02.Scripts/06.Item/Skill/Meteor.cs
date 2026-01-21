using Sirenix.OdinInspector;
using UnityEngine;

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
    float m_scalefactor;

    public override void OnSpawn()
    {
        base.OnSpawn();
        m_scalefactor = 1;//transform.parent.localScale.x;
        time = Time.time;
        ishit = false;
    }

    void LateUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed * m_scalefactor);
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
        
        m_makedObject = Instantiate(m_hitObject, hit.point, Quaternion.LookRotation(hit.normal)).gameObject;
        m_makedObject.transform.parent = transform.parent;
        m_makedObject.transform.localScale = new Vector3(1, 1, 1);

        if (!m_makedObject.TryGetComponent(out ExplosionSkill explosion)) return;
        explosion.Explosion(_damage.GetRandomValue());
    }

    void MakeHitObject(Transform point)
    {
        if (isHitMake == false)
            return;
        
        var m_makedObject = PoolManager.Instance.Get(_meteorHit);
        
        m_makedObject = Instantiate(m_hitObject, point.transform.position, point.rotation).gameObject;
        m_makedObject.transform.parent = transform.parent;
        m_makedObject.transform.localScale = new Vector3(1, 1, 1);
        
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
