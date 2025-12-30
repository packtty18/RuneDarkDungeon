public enum EGameState
{ 
    Ready,
    Playing,
    GameOver
}

public enum EEnemyState
{ 
    Spawn,
    Idle,
    Patrol,
    Trace,
    Comeback,
    Attack,
    Hit,
    Death,
    RageOn,
    RageMove,
    RageAttack
}


#region PlayerStat
public enum EConsumableFloat
{
    Health,
    Stamina,
}

public enum EConsumableInt
{
    InvenBulletCount,
    LoadedBulletCount,
    BombCount
}

public enum EValueFloat
{
    //HP
    MaxHealth,
    HealthRegenPerSecond,
    //Move
    MaxStamina,
    StaminaRegenPerSecond,
    StaminaRegenDelay,
    DashConsumeStaminaPerSecond,
    DoubleJumpConsumeStaminaPerOnce,
    MoveSpeed,
    DashSpeed,
    JumpPower,
    //gun
    GunDamage,
    GunFireDelay,
    GunMaxRange,
    GunReloadTime,
    GunKnockbackPower,
    //Recoil
    GunRecoilX,
    GunRecoilY,
    //Bomb
    BombDamage,
    BombRadius,
    BombThrowForce,
    BombThrowDelay,
    BombKnockbackPower
}

public enum EValueInt
{
    //gun
    GunMaxBullet,

    //Bomb
    BombMaxCount,
}
#endregion