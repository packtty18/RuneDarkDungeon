using Sirenix.OdinInspector;
using UnityEngine;

public enum ESoundType
{
    None = 0,

    Bgm_Test1,
    Bgm_Test2,

    Sfx_Test1,
    Sfx_Test2,
    Sfx_Test3,
    
    UI_Button_Click,
    Rune_Sell,
    Gold_Get,
    
    Skill_BladeStorm,
    Skill_Lightningzone,
    Skill_JudgmentMeteor,

    Enemy_Warrior_Footstep,
    Enemy_Warrior_Attack,
    Enemy_Warrior_Hit,
    Enemy_Warrior_Death,

    Enemy_Archer_Footstep,
    Enemy_Archer_Attack,
    Enemy_Archer_Hit,
    Enemy_Archer_Death,

    Enemy_Mage_Footstep,
    Enemy_Mage_Attack,
    Enemy_Mage_Hit,
    Enemy_Mage_Death,

    Enemy_Elite_Footstep,
    Enemy_Elite_Attack,
    Enemy_Elite_Hit,
    Enemy_Elite_Death,
    Enemy_Elite_Charge,

    Enemy_Boss_Footstep,
    Enemy_Boss_Attack,
    Enemy_Boss_Hit,
    Enemy_Boss_Death,
    Enemy_Boss_Charge,
    Enemy_Boss_Summon,
    Enemy_Boss_Buff,
}

public class SoundManager : GlobalSingleton<SoundManager>
{
    [SerializeField] private SoundDatabaseSO _database;

    [SerializeField] private AudioSource _bgmSource;
    private SoundFactory _soundFactory;
    

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

    //BGM 오디오 소스 생성
    //BGM은 오직 하나만 생성되며 씬이 변경되더라도 해당 매니저에 붙어 파괴되지 않음.
    private void CreateBgmSource()
    {
        GameObject go = new GameObject("BGM");
        go.transform.SetParent(transform);

        _bgmSource = go.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        
    }

    public void Play(ESoundType key, Vector3 position = default)
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
        _bgmSource.volume = data.volume;
        _bgmSource.spatialBlend = 0;
        _bgmSource.Play();
    }

    private void PlayInternalSfx(SoundData data, Vector3 position)
    {
        _soundFactory.Play(data, position);
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

    [ShowIf(nameof(_test))]
    [Button]
    public void StopBGM()
    {
        _bgmSource.Stop();
        _bgmSource.clip = null;
    }

    #endregion
#endif

}

