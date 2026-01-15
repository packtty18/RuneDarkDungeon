using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;


//아이템의 베이스
//골드 ,룬 등
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class ItemBase : PoolableObject
{
    [Title("Refrence")]
    [SerializeField] protected Rigidbody _rigid;
    [SerializeField] protected Collider _physic;
    [SerializeField] protected Collider _trigger;
    [SerializeField] protected Transform _target;

    [Title("Spawn")]
    [SerializeField] protected float _spawnForce = 10f;        //스폰시 위로 튕기는 힘
    [SerializeField] protected float _spawnRotateForce = 30f;   //스폰시 회전하는 힘

    [Title("Detect & Attract")]
    [SerializeField] protected float _detectDelay = 0.5f;       //생성후 대기시간
    [SerializeField] protected float _attractDistance = 10f;    //플레이어를 감지하는 거리
    [SerializeField] protected float _attractSpeed = 8f;        //이동 속도
    [SerializeField] protected float _curveHeight = 3f;         // 베지어 곡선의 높이

    protected float _spawnTime;         //생성 시간 캐싱
    protected bool _isAttracting;       //현재 플레이어를 향해 이동하는지
    protected Tween _attractTween;      //트위닝 캐싱

    private Vector3 _startPosition;
    private Vector3 _controlPosition;

    protected virtual void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (_target == null)
        {
            return;
        }

        if (_isAttracting)
        {
            return;
        }
        if (Time.time - _spawnTime < _detectDelay)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _target.position);
        if (distance <= _attractDistance)
        {
            StartAttract();
        }
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        if(BattleManager.IsExist())
        {
            _target = BattleManager.Instance?.transform;
        }

        ResetState();
        AddSpawnForce();

        _spawnTime = Time.time;

        Debug.Log($"{name} Get");
    }

    public override void OnDespawn()
    {
        KillTween();
        ResetPhysics();

        _isAttracting = false;
        base.OnDespawn();

        Debug.Log($"{name} Release");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Collect();
    }

    protected virtual void StartAttract()
    {
        _isAttracting = true;
        SetPhysics(false);

        Debug.Log($"{name} Attract Start");

        _startPosition = transform.position;
        
        Vector3 midPoint = (_startPosition + _controlPosition) / 2f;
        Vector3 randomOffset = Random.insideUnitSphere;
        randomOffset.y = Mathf.Abs(randomOffset.y);
        
        _controlPosition = midPoint + (Vector3.up * _curveHeight) + randomOffset;
        
        AttractStep();
    }

    protected void AttractStep()
    {
        if (_target == null)
        {
            return;
        }

        float distance = Vector3.Distance(_startPosition, _target.position);
        float duration = Mathf.Clamp(distance / _attractSpeed, 0.05f, 0.2f);

        float time = 0f;
        
        _attractTween = DOTween.To(() => time, x => time = x, 1f, duration)
            .SetEase(Ease.InQuad)
            .OnUpdate(() =>
            {
                transform.position = CalculateQuadraticBezierPoint(time, _startPosition, _controlPosition, _target.position);
            });
    }
    
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // (1-t)^2 * P0
        p += 2 * u * t * p1; // 2(1-t)t * P1
        p += tt * p2;        // t^2 * P2

        return p;
    }
    
    protected virtual void Collect()
    {
        KillTween();
        OnCollected();
        ReturnToPool();
    }

    //여기에 데이터 처리 확장
    protected abstract void OnCollected();

    [Button]
    protected void AddSpawnForce()
    {
        Vector3 dir = Vector3.up + Random.insideUnitSphere * 0.5f;
        dir.Normalize();

        _rigid.AddForce(dir * _spawnForce, ForceMode.Impulse);
        _rigid.angularVelocity = Random.onUnitSphere * _spawnRotateForce;
    }

    protected void ResetState()
    {
        KillTween();
        ResetPhysics();
    }

    protected void ResetPhysics()
    {
        //초기화
        SetPhysics(true);
        _rigid.linearVelocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }

    protected void SetPhysics(bool enable)
    {
        //충돌 활성화
        //트리거 비활성화

        _rigid.isKinematic = !enable;
        _physic.enabled = enable;
        _trigger.enabled = !enable;
    }

    protected void KillTween()
    {
        if (_attractTween != null)
        {
            _attractTween.Kill();
            _attractTween = null;
        }
    }
}