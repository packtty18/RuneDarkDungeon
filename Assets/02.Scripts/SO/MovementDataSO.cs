using UnityEngine;

[CreateAssetMenu(menuName = "Stat/Movement Data")]
public class MovementDataSO : ScriptableObject
{
    [Header("Movement")]
    public float MoveSpeed;     // 이동속도
    public float DashSpeed;     // 대쉬 상태 이동속도
    public float JumpPower;     // 점프 힘

    [Header("Stamina")]
    public float MaxStamina;    // 최대 스태미나
    public float StaminaRegen;  // 초당 스태미나 회복량
    public float RegenDelay;    // 소모 종료 후 회복 시작까지의 딜레이

    [Header("Stamina Consume")]
    public float DashConsume;        // 대쉬 시 초당 소모
    public float DoubleJumpConsume;  // 더블 점프 시 소모
}