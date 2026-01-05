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
        
        _soundFactory = new SoundFactory(PoolManager.Instance, "SoundObject");
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
        SoundObject obj = _soundFactory.CreateAt(position);

        obj.Play(data);
    }

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


}

