using UnityEngine;

[CreateAssetMenu(menuName = "WeaponStat/Bomb Data")]
public class BombDataSO : ScriptableObject
{
    public int MaxCount;            //최대 소유 개수

    public float Damage;            //데미지
    public float ExplosionRadius;   //폭발 반경
    public float Force;             //폭탄을 던지는 힘
    public float Delay;             //다음 폭탄을 던지기 까지 딜레이

    public KnockbackData Knockback; //넉백에 대한 정보
}
