public enum EGameState
{ 
    Ready,
    Playing,
    GameOver
}

public enum EAttackType
{
    Basic, 
    Jump,   
}

public enum EMovementState
{
    Idle,
    Walk,
    Run,
    Jump,
    Stagger, //경직   
    Dead,
}

public enum EActionState
{
    None,
    Attack,
    DashAttack,
    Skill,
}

public enum EEnemyState
{
    Idle,
    Chase,
    Attack,
    Hit,
    Dead,
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
    SFX,
    Enemy_Warrior,
    Enemy_Archer,
    Enemy_Mage,
    Enemy_Elite,
    Enemy_Boss,
    Item_Coin,
    Item_Rune
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