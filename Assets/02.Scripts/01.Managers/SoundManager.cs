using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundType
{
    None = 0,

    Bgm_Test1,
    Bgm_Test2,
    Bgm_Lobby,
    Bgm_Main,
    Bgm_Game,

    Sfx_Test1,
    Sfx_Test2,
    Sfx_Test3,
    
    UI_Button_Click,
    Rune_Sell,
    Gold_Get,

    Player_Walk,
    Player_BasicAttack1,
    Player_BasicAttack2,
    Player_DashAttack,
    Player_Dash,
    Player_FinisherAttack,
    Player_Land,

    Player_Footstep1,
    Player_Footstep2,
    Player_Footstep3,
    Player_Footstep4,
    Player_Footstep5,

    Player_Grunt1,
    Player_Grunt2,
    Player_Grunt3,
    Player_Grunt4,
    Player_Grunt5,
    
    Skill_BladeStorm,
    Skill_Lightningzone,
    Skill_JudgmentMeteor,

    Enemy_Common_Hit,
    Enemy_Common_Dead,
    Enemy_Warrior_Slash,
    Enemy_Archer_Shoot,
    Enemy_Mage_Cast,
    Ememt_Mage_Shoot,
    Enemy_Mage_Explosion,

    Enemy_Elite_Slash,
    Enemy_Elite_Hit,
    Enemy_Elite_Death,
    Enemy_Elite_Charge,

    Enemy_Boss_Slash,
    Enemy_Boss_MagicMissile,
    Enemy_Boss_Lightening,
    Enemy_Boss_BloodExplosion,
    Enemy_Boss_Hit,
    Enemy_Boss_Death,
    Enemy_Boss_Charge,
    Enemy_Boss_Summon,
    Enemy_Boss_Buff,

    Prop_hit,
    Prop_Destroy1,
    Prop_Destroy2,

    Explosion1,
    Explosion2,
}

public class SoundManager : GlobalSingleton<SoundManager>
{
    [SerializeField] private SoundDatabaseSO _database;

    [SerializeField] private AudioSource _bgmSource;
    private SoundFactory _soundFactory;

    [Header("Sound Cooldown Settings")]
    [SerializeField] private float _soundCooldown = 1f; // 같은 사운드 최소 간격
    private Dictionary<ESoundType, float> _lastPlayTime = new Dictionary<ESoundType, float>();


    private float _globalBgmVolume = 1f;
    private float _globalSfxVolume = 1f;
    public float GlobalBgmVolume => _globalBgmVolume;
    public float GlobalSfxVolume => _globalSfxVolume;

    protected override void Awake()
    {
        base.Awake();
        _database.Init();
        if(_bgmSource == null) 
        {
            CreateBgmSource();
        }
    }

    private void Start()
    {
        _soundFactory = new SoundFactory(PoolManager.Instance, EPoolType.SFX);
    }

    public void SetBgmVolume(float volume)
    {
        _globalBgmVolume = Mathf.Clamp01(volume);

        if (_bgmSource == null || !_bgmSource.isPlaying) return;
        _bgmSource.volume = _globalBgmVolume;
    }

    public void SetSfxVolume(float volume)
    {
        _globalSfxVolume = Mathf.Clamp01(volume);
    }
    
    //BGM 오디오 소스 생성
    //BGM은 오직 하나만 생성되며 씬이 변경되더라도 해당 매니저에 붙어 파괴되지 않음.
    private void CreateBgmSource()
    {
        GameObject go = new GameObject("BGM");
        go.transform.SetParent(transform);

        _bgmSource = go.AddComponent<AudioSource>();
        _bgmSource.loop = true;
    }

    public void Play(ESoundType key, Vector3 position = default, bool playImmediate = true)
    {
        if (_database.TryGet(key, out var data) == false)
        {
            return;
        }

        if (data.isBgm == true)
        {
            PlayInternalBgm(data);
        }
        else
        { 
            if (!playImmediate && _lastPlayTime.TryGetValue(key, out float lastTime))
            {
                if (Time.time - lastTime < _soundCooldown)
                    return;
            }

            _lastPlayTime[key] = Time.time;
            PlayInternalSfx(data, position);
        }
    }

    private void PlayInternalBgm(SoundData data)
    {
        if (_bgmSource == null)
        {
            CreateBgmSource();
        }

        _bgmSource.clip = data.clip;
        _bgmSource.volume = data.volume * _globalBgmVolume;
        _bgmSource.spatialBlend = 0;
        _bgmSource.Play();
    }

    private void PlayInternalSfx(SoundData data, Vector3 position)
    {
        data.volume *= _globalSfxVolume;
        
        _soundFactory.Play(data, position);
    }

    [Button]
    public void StopBGM()
    {
        _bgmSource.Stop();
        _bgmSource.clip = null;
    }

#if UNITY_EDITOR
    #region Test
    [SerializeField] private bool _test = false;

    [Header("Test - Common")]
    [ShowIf(nameof(_test))]
    [SerializeField] private ESoundType _testSoundType = ESoundType.None;

    [Header("Test - SFX")]
    [ShowIf(nameof(_test))]
    [SerializeField] private Vector3 _testSfxPosition = Vector3.zero;

    [ShowIf(nameof(_test))]
    [Button]
    public void TestPlaySound()
    {
        Play(_testSoundType, _testSfxPosition);
    }

    #endregion
#endif

}

