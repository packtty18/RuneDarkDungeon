using UnityEngine;

public interface IDamageable 
{
    //데미지를 적용. 해당 객체가 사망처리될지 판정
    // true  → 살아있음 (Hit)
    // false → 사망
    void ApplyDamage(AttackData data);
}
