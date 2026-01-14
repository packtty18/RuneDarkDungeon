using System.Collections;
using UnityEngine;

public class LightningZoneSkill : SkillBase
{
    [Header("지속 피해 간격")]
    [SerializeField] private float _tick = 0.25f;
    
    [Header("레전드 추가 스킬")]
    [SerializeField] private GameObject _explosionPrefab;
    
    [Header("이펙트 오프셋")]
    [SerializeField] private Vector3 _offset;

    protected override void ApplyEffect(GameObject user, EItemGrade grade)
    {
        transform.position += _offset;
        
        StartCoroutine(TickDamageRoutine());

        if (grade < EItemGrade.Unique) return;
        transform.SetParent(user.transform);
    }
    
    private IEnumerator TickDamageRoutine()
    {
        var hitbox = GetComponent<HitBox>();
        float timer = 0;
        
        WaitForSeconds waitTick = new(_tick);
        
        while (timer < _lifeTime)
        {
            hitbox.Activate(_damage);
            yield return waitTick;
            timer += _tick;
        }
    }

    private void OnDestroy()
    {
        if (_grade != EItemGrade.Legendary) return;
        Instantiate(_explosionPrefab, _user.transform.position, Quaternion.identity);
    }
}
