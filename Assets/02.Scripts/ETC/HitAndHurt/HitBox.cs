using Sirenix.OdinInspector;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

// 히트박스는 공격 판정을 담당.
// 추후 애니메이터와 연결
[RequireComponent(typeof(Collider))]
public class HitBox : MonoBehaviour
{
    private static int s_globalAttackId = 0;
    public static void ResetId()
    {
        s_globalAttackId = 0;
    }

    [SerializeField] private int _currentAttackId;

    private Collider _collider;
    private bool _isActive;

    [Title("공격자 팀")]
    [SerializeField] 
    private ETeamType _team;

    [Title("데미지")]
    [SerializeField] 
    private float _damage = 10f;

    [Title("단발성 히트박스 제어")]
    //애니메이션을 통해 제어한다면 false, 폭발 같은 단발성일 경우 true
    [SerializeField] 
    private bool _autoDeactive = false;
    [SerializeField,ShowIf(nameof(_autoDeactive))] 
    private float _deactiveDelay = 0.2f;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        _isActive = false;
    }


    [Button("Activate HitBox")]
    public void Activate(float damage = 0)
    {
        if (_isActive)
        {
            return;
        }

        _currentAttackId = ++s_globalAttackId;
        _damage = damage;

        _isActive = true;
        _collider.enabled = true;

        if(_autoDeactive)
        {
            Invoke(nameof(Deactivate), _deactiveDelay);
        }
    }

    [Button("Deactivate HitBox")]
    public void Deactivate()
    {
        if (!_isActive)
            return;

        _isActive = false;
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActive)
        {
            return;
        }
        if (!other.TryGetComponent(out HurtBox hurtBox))
        {
            return;
        }

        Vector3 hitPoint = other.ClosestPoint(transform.position);
        Vector3 hitDirection = (hitPoint - transform.position).normalized;

        DamageData damageData = new DamageData
        {
            AttackId = _currentAttackId,
            Damage = _damage,
            Team = _team,
            HitDirection = hitDirection,
        };

        hurtBox.ApplyDamage(damageData);
    }
}