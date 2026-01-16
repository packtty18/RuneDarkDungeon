using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class EnemyAttackObject : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private AttackObjectDataSO data;

    [SerializeField] private IAttackMovement _movement;
    [SerializeField] private IAttackTrigger _trigger;
    [SerializeField] private float _damage;
    
    //해당 공격을 만든자
    public EnemyController Owner { get; private set; }
    public float Damage => _damage;

    //해당 공격오브젝트가 소유한 히트박스, 없다면 비우기
    [SerializeField] private HitBox _hitBox;
    [SerializeField] private bool _initHitbox;

    private bool init = false;

    private void OnEnable()
    {
        DeactivateHitBox();
    }
    [Button]
    public void Initialize(EnemyController owner, float damage = 0)
    {
        Owner = owner;

        _movement = GetComponent<IAttackMovement>();
        _trigger = GetComponent<IAttackTrigger>();

        _movement?.Initialize(data);
        _trigger?.Initialize(this, data);

        //데미지 등록
        _damage = damage;

        //자동 파괴
        if (data.LifeTime > 0f)
        {
            StartCoroutine(Util.DestroyAfterTime(data.LifeTime, gameObject));
        }
        else
        {
            Util.ObjectDestroy(gameObject);
        }

        //히트박스
        SetHitbox();

        init = true;
    }

    private void SetHitbox()
    {
        if (_hitBox != null)
        {
            if (_initHitbox)
            {
                _hitBox.Activate(_damage);
            }
            else
            {
                _hitBox.Deactivate();
            }
        }
    }

    private void Update()
    {
        _movement?.Tick(Time.deltaTime);
    }

    private void OnDisable()
    {
        init = false;
    }


    public void ExecuteEffects(Vector3 position)
    {
        foreach (var effect in data.Effects)
        {
            effect?.Execute(this, position);
        }
    }

    public void ActivateHitBox(Vector3 position)
    {
        if (_hitBox == null)
        {
            Debug.LogWarning("[EnemyAttackObject] HitBox is missing");
            return;
        }

        _hitBox.transform.position = position;
        _hitBox.Activate(_damage);

        Debug.Log("[EnemyAttackObject] HitBox activated");
    }

    public void DeactivateHitBox()
    {
        if (_hitBox == null)
            return;

        _hitBox.Deactivate();
        Debug.Log("[EnemyAttackObject] HitBox deactivated");
    }

    public void StartSpawnRoutine(
        GameObject prefab,
        Vector3 position,
        int count,
        float delay)
    {
        StartCoroutine(SpawnCoroutine(prefab, position, count, delay));
    }

    private IEnumerator SpawnCoroutine(
        GameObject prefab,
        Vector3 position,
        int count,
        float delay)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, position, Quaternion.identity);

            EnemyAttackObject attackObj =
                obj.GetComponent<EnemyAttackObject>();

            attackObj?.Initialize(Owner);

            Debug.Log($"[EnemyAttackObject] Spawned {i + 1}/{count}");

            if (delay > 0f)
                yield return new WaitForSeconds(delay);
        }
    }
}
