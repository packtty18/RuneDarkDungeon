using UnityEngine;

public class HurtHealth : MonoBehaviour,IDamageable
{
    [SerializeField] private ETeamType team;
    public ETeamType Team => team;

    public void ApplyDamage(DamageData data)
    {
        //체력 상태에 따른 데미지 처리 로직 추가
        Debug.Log($"[{gameObject.name}] : 데미지");
    }

   
}
