using UnityEngine;

[CreateAssetMenu(menuName = "WeaponStat/Gun Data")]
public class GunDataSO : ScriptableObject
{
    public int MaxBullet;    //탄창의 크기

    public float Damage;    //총의 데미지
    public float FireDelay; //총의 발사 간격
    public float Range;     //사거리

    public float ReloadTime;    //재장전 시간

    public KnockbackData KnockbackData; //넉백 정보
    public RecoilData RecoilData;       //반동 정보
}
