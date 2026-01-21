using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

//사운드는 피격음을 제외하면 State나 애니메이션에서 호출
public class EnemySound : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyController _controller;
    [SerializeField] private EnemyStat _stat;

    private EEnemyType _enemyType;
    private bool _isInitialized = false;

    // 적 타입별 사운드 매핑
    private Dictionary<EEnemyType, EnemySoundSet> _soundSets;

    private void Awake()
    {
        if (_controller == null)
            _controller = GetComponent<EnemyController>();
        
        if (_stat == null)
            _stat = GetComponent<EnemyStat>();
    }

    public void Init()
    {
        _enemyType = _stat.EnemyType;
        InitializeSoundSets();
        _isInitialized = true;
    }

    private void InitializeSoundSets()
    {
        _soundSets = new Dictionary<EEnemyType, EnemySoundSet>
        {
            {
                EEnemyType.Warrior, new EnemySoundSet
                {
                    Attack = ESoundType.Enemy_Warrior_Slash,
                    Hit = ESoundType.Enemy_Common_Hit,
                    Death = ESoundType.Enemy_Common_Dead,
                }
            },
            {
                EEnemyType.Archer, new EnemySoundSet
                {
                    Attack = ESoundType.Enemy_Archer_Shoot,
                    Hit = ESoundType.Enemy_Common_Hit,
                    Death = ESoundType.Enemy_Common_Dead,
                }
            },
            {
                EEnemyType.Mage, new EnemySoundSet
                {
                    Cast = ESoundType.Enemy_Mage_Cast,
                    Attack = ESoundType.Ememt_Mage_Shoot,
                    Hit = ESoundType.Enemy_Common_Hit,
                    Death = ESoundType.Enemy_Common_Dead,
                }
            },
            {
                EEnemyType.Elite, new EnemySoundSet
                {
                    Attack = ESoundType.Enemy_Elite_Slash,
                    Death = ESoundType.Enemy_Elite_Death,
                    Hit = ESoundType.Enemy_Common_Hit,
                    Charge = ESoundType.Enemy_Elite_Charge
                }
            },
            {
                EEnemyType.Boss, new EnemySoundSet
                {
                    Cast = ESoundType.Enemy_Mage_Cast,
                    Attack = ESoundType.Enemy_Boss_Slash,
                    Charge = ESoundType.Enemy_Boss_Charge,
                    Summon = ESoundType.Enemy_Boss_Summon,
                    Hit = ESoundType.Enemy_Common_Hit,
                    Death = ESoundType.Enemy_Boss_Death,
                }
            }
        };
    }

    #region 애니메이션 혹은 state

    //애니메이터에서 적용
    [Button("Test Attack")]
    public void PlayAttack()
    {
        if (!_isInitialized) return;

        if (_soundSets.TryGetValue(_enemyType, out var soundSet))
        {
            PlaySound(soundSet.Attack);
        }
    }

    [Button("Test Cast")]
    public void PlayCast()
    {
        if (!_isInitialized) return;

        if (_soundSets.TryGetValue(_enemyType, out var soundSet))
        {
            PlaySound(soundSet.Cast);
        }
    }


    [Button("Test Death")]
    public void PlayDeath()
    {
        if (!_isInitialized) return;

        if (_soundSets.TryGetValue(_enemyType, out var soundSet))
        {
            PlaySound(soundSet.Death); 
        }
    }

    [Button("Test Hit")]
    public void PlayHit()
    {
        if (!_isInitialized) return;

        if (_soundSets.TryGetValue(_enemyType, out var soundSet))
        {
            PlaySound(soundSet.Hit,false); 
        }
    }

    [Button("Test Charge")]
    public void PlayCharge()
    {
        if (!_isInitialized) return;

        if (_soundSets.TryGetValue(_enemyType, out var soundSet) && soundSet.Charge != ESoundType.None)
        {
            PlaySound(soundSet.Charge);
        }
    }

    [Button("Test Summon")]
    public void PlaySummon()
    {
        if (!_isInitialized) return;

        if (_soundSets.TryGetValue(_enemyType, out var soundSet) && soundSet.Summon != ESoundType.None)
        {
            PlaySound(soundSet.Summon);
        }
    }

    [Button("Test Buff")]
    public void PlayBuff()
    {
        if (!_isInitialized) return;

        if (_soundSets.TryGetValue(_enemyType, out var soundSet) && soundSet.Buff != ESoundType.None)
        {
            PlaySound(soundSet.Buff);
        }
    }

    #endregion

    //쿨다운 적용
    private void PlaySound(ESoundType soundType, bool immediate = true)
    {
        if (soundType == ESoundType.None)
            return;

        // SoundManager를 통해 3D 사운드 재생
        if (SoundManager.IsExist())
        {
            SoundManager.Instance.Play(soundType, transform.position, immediate);
        }
    }
}

/// <summary>
/// 적 타입별 사운드 세트
/// </summary>
[System.Serializable]
public class EnemySoundSet
{
    public ESoundType Attack = ESoundType.None;
    public ESoundType Cast = ESoundType.None;
    public ESoundType Hit = ESoundType.None;
    public ESoundType Death = ESoundType.None;
    public ESoundType Charge = ESoundType.None;
    public ESoundType Summon = ESoundType.None;
    public ESoundType Buff = ESoundType.None;
}
