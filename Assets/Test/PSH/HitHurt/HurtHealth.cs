using UnityEngine;

public class HurtHealth : MonoBehaviour,IDamageable
{
    [SerializeField] private ETeamType team;
    public ETeamType Team => team;

    public void ApplyDamage(DamageData data)
    {
        //체력 상태에 따른 데미지 처리 로직 추가
        int dir = DirectionConvert(data.HitDirection);
        

        //Debug.Log($"{gameObject.name} 피격, {data.AttackId}, {data.HitDirection}, {dir}");
    }
    private int DirectionConvert(Vector3 hitDirection)
    {
        Vector3 localDir = transform.InverseTransformDirection(hitDirection);
        localDir.y = 0f;

        // Decide by dominant axis
        if (Mathf.Abs(localDir.x) > Mathf.Abs(localDir.z))
        {
            return localDir.x > 0f ? (int)EHitDirection.Right : (int)EHitDirection.Left;
        }

        return localDir.z > 0f ? (int)EHitDirection.Front : (int)EHitDirection.Back;
    }

}
