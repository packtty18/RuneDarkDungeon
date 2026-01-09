public enum EGameState
{ 
    Ready,
    Playing,
    GameOver
}

public enum EAttackType
{
    Ground, 
    Air   
}

public enum EPlayerState
{
    Idle,
    Walk,
    Run,
    Jump,
    Attack,
    Skill,
    Dodge,
    Hit,
    Dead,
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

public enum EGameKeyType
{
    Front,
    Back,
    Left,
    Right,
    Jump,
    Attack,
    Run,
    Enter,
}

public enum ESceneType
{
    MainMenu,
    Gameplay,
    Lobby,
    Loading,
}

public enum ETeamType
{
    Player,
    Enemy,
    Neutral
}

public enum EItemGrade
{
    Normal,
    Rare,
    Unique,
    Legendary
}

public enum EInventoryMode
{
    Closed,
    Normal,
    Upgrade,
}

public enum  EPoolType
{
    None,
    Box,
    Sound,
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