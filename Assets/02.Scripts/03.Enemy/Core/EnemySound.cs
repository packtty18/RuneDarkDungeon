using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

//사운드는 피격음을 제외하면 State나 애니메이션에서 호출
public class EnemySound : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyController _controller;
    [SerializeField] private EnemyStat _stat;

    [Header("Footstep Settings")]
    [SerializeField] private float _footstepCooldown = 0.3f; // 발자국 최소 간격
    private float _lastFootstepTime;

    [Header("Sound Cooldown Settings")]
    [SerializeField] private float _soundCooldown = 0.1f; // 같은 사운드 최소 간격
    private Dictionary<ESoundType, float> _lastPlayTime = new Dictionary<ESoundType, float>();

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
                }
            },
            {
                EEnemyType.Archer, new EnemySoundSet
                {
                    Attack = ESoundType.Enemy_Archer_Shoot,
                }
            },
            {
                EEnemyType.Mage, new EnemySoundSet
                {
                    Attack = ESoundType.Enemy_Mage_Fireball,
                }
            },
            {
                EEnemyType.Elite, new EnemySoundSet
                {
                    Attack = ESoundType.Enemy_Elite_Slash,
                    Death = ESoundType.Enemy_Elite_Death,
                    Charge = ESoundType.Enemy_Elite_Charge
                }
            },
            {
                EEnemyType.Boss, new EnemySoundSet
                {
                    Attack = ESoundType.Enemy_Boss_Slash,
                    Death = ESoundType.Enemy_Boss_Death,
                    Charge = ESoundType.Enemy_Boss_Charge,
                    Summon = ESoundType.Enemy_Boss_Summon,
                }
            }
        };
    }

    #region 애니메이션 혹은 state


    [Button("Test Attack")]
    public void PlayAttack()
    {
        if (!_isInitialized) return;

        if (_soundSets.TryGetValue(_enemyType, out var soundSet))
        {
            PlaySound(soundSet.Attack);
        }
    }

    [Button("Test Death")]
    public void PlayDeath()
    {
        if (!_isInitialized) return;

        if (_soundSets.TryGetValue(_enemyType, out var soundSet))
        {
            PlaySoundImmediate(soundSet.Death); // 사망음은 쿨다운 무시
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
    private void PlaySound(ESoundType soundType)
    {
        if (soundType == ESoundType.None)
            return;

        // 쿨다운 체크
        if (_lastPlayTime.TryGetValue(soundType, out float lastTime))
        {
            if (Time.time - lastTime < _soundCooldown)
                return;
        }

        _lastPlayTime[soundType] = Time.time;

        // SoundManager를 통해 3D 사운드 재생
        if (SoundManager.IsExist())
        {
            SoundManager.Instance.Play(soundType, transform.position);
        }
    }
    // 사운드 즉시 재생 (쿨다운 무시) - 사망음 등 중요한 사운드용
    private void PlaySoundImmediate(ESoundType soundType)
    {
        if (soundType == ESoundType.None)
            return;

        if (SoundManager.IsExist())
        {
            SoundManager.Instance.Play(soundType, transform.position);
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
    public ESoundType Death = ESoundType.None;
    public ESoundType Charge = ESoundType.None;
    public ESoundType Summon = ESoundType.None;
    public ESoundType Buff = ESoundType.None;
}
