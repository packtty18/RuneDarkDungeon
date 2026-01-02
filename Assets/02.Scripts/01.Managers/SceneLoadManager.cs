using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 씬 로딩,전환 및 씬 데이터를 관리하는 싱글톤 매니저
/// <para>사용법: SceneDataSO.LoadScene() 또는 SceneLoadManager.Instance.LoadLoading(sceneData) 호출</para>
/// </summary>
public class SceneLoadManager : GlobalSingleton<SceneLoadManager>
{
    #region Events
    /// <summary>씬 로딩 시작 시 호출되는 이벤트</summary>
    public event Action<string> OnSceneLoadStart;
        
    /// <summary>씬 로딩 진행률 업데이트 이벤트 (0.0 ~ 1.0)</summary>
    public event Action<float> OnSceneLoadProgress;
        
    /// <summary>씬 로딩 완료 시 호출되는 이벤트</summary>
    public event Action<string> OnSceneLoadComplete;
        
    /// <summary>씬 전환 시작 시 호출되는 이벤트</summary>
    public event Action OnTransitionStart;
        
    /// <summary>씬 전환 완료 시 호출되는 이벤트</summary>
    public event Action OnTransitionComplete;
    #endregion

    private bool _isLoading = false;
    private float _loadingProgress = 0f;
    private SceneDataSO _currentSceneData;
    private SceneDataSO _nextSceneData; // 로드할 씬 데이터
    private ESceneType _sceneType;

    [SerializeField]
    private string _loadingSceneName = "LoadingScene";
    private Dictionary<string, object> _sceneData = new Dictionary<string, object>();


    public SceneDataSO StartSceneData;
    public bool IsLoading => _isLoading;
    public float LoadingProgress => _loadingProgress;

    /// <summary>전환하고자하는 씬의 데이터를 가져옵니다.</summary>
    public SceneDataSO NextSceneData => _nextSceneData;

    /// <summary>현재 씬의 데이터를 가져옵니다.</summary>
    public SceneDataSO CurrentSceneData => _currentSceneData;

    /// <summary>현재 씬 타입을 가져옵니다.</summary>
    public ESceneType SceneType => _sceneType;

    protected override void Awake()
    {
        base.Awake();
        _currentSceneData = StartSceneData;
        _sceneType = _currentSceneData.SceneType;
    }

    #region Public Methods - Scene Loading
    /// <summary>
    /// 씬을 비동기로 로드합니다 (기존 씬은 언로드)
    /// Asynchronously loads a scene (unloads previous scene)
    /// </summary> 
    public void BeginSceneLoad(SceneDataSO dataSO)
    {
        if (_isLoading)
        {
            Debug.LogWarning($"[SceneLoadManager] 이미 로딩 중입니다. Already loading: {_currentSceneData.name}");
            return;
        }
        _nextSceneData = dataSO;
        //로딩씬 전환
        UnityEngine.SceneManagement.SceneManager.LoadScene(_loadingSceneName);
    }

    public void LoadTargetScene()
    {
        if (_nextSceneData == null)
        {
            Debug.LogWarning($"[SceneLoadManager] 로드할 씬 데이터가 없습니다.");
            return;
        }
        StartCoroutine(LoadSceneAsync());
    }


    /// <summary>
    /// 현재 씬을 다시 로드합니다
    /// </summary>
    public void ReloadCurrentScene()
    {
        if (_currentSceneData != null)
        {
            BeginSceneLoad(_currentSceneData);
        }
        else
        {
            Debug.LogWarning("[SceneLoadManager] 현재 씬 데이터가 없어 재로드할 수 없습니다.");
        }
    }

    /// <summary>
    /// 이전 씬으로 돌아갑니다.
    /// </summary>
    private void LoadFallbackScene()
    {
        Debug.LogWarning($"폴백 씬으로 이동: {_currentSceneData.SceneName}");
        SceneManager.LoadScene(_currentSceneData.SceneName);
    }
    #endregion

    #region Public Methods - Scene Data
    /// <summary>
    /// 씬 전환 시 전달할 데이터를 설정합니다
    /// </summary>
    public void SetSceneData(string key, object value)
    {
        if (_sceneData.ContainsKey(key))
        {
            _sceneData[key] = value;
        }
        else
        {
            _sceneData.Add(key, value);
        }
    }

    /// <summary>
    /// 씬 데이터를 가져옵니다
    /// </summary>
    public T GetSceneData<T>(string key, T defaultValue = default)
    {
        if (_sceneData.TryGetValue(key, out object value))
        {
            try
            {
                return (T)value;
            }
            catch (InvalidCastException)
            {
                Debug.LogError($"[SceneLoadManager] 데이터 타입 변환 실패. Failed to cast data: {key}");
                return defaultValue;
            }
        }

        return defaultValue;
    }

    /// <summary>
    /// 씬 데이터를 삭제합니다
    /// </summary>
    /// <param name="key">데이터 키</param>
    public void RemoveSceneData(string key)
    {
        if (_sceneData.ContainsKey(key))
        {
            _sceneData.Remove(key);
        }
    }

    /// <summary>
    /// 모든 씬 데이터를 초기화합니다
    /// </summary>
    public void ClearSceneData()
    {
        _sceneData.Clear();
    }
    #endregion

    #region Private Couroutine LoadSceneAsync
    private IEnumerator LoadSceneAsync()
    {
        _isLoading = true;
        _loadingProgress = 0f;
        float startTime = Time.time;
        string sceneName = _nextSceneData.SceneName;
        float minimumLoadTime = _nextSceneData.MinimumLoadTime;


        // 전환 시작 이벤트 발생
        OnTransitionStart?.Invoke();
        OnSceneLoadStart?.Invoke(sceneName);

        // 비동기 씬 로드 시작
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            
        if (asyncLoad == null)
        {
            Debug.LogError($"[SceneLoadManager] 씬 로드 실패. Failed to load scene: {sceneName}");
            _isLoading = false;
            _sceneType = _currentSceneData.SceneType;
            _nextSceneData = null;

            LoadFallbackScene();
            yield break;
        }

        // 로딩이 완료되어도 자동으로 활성화되지 않도록 설정
        asyncLoad.allowSceneActivation = false;

        // 로딩 진행률 업데이트
        while (asyncLoad.progress < 0.9f)
        {
            _loadingProgress = asyncLoad.progress;
            OnSceneLoadProgress?.Invoke(_loadingProgress);
            yield return null;
        }

        // 최소 로딩 시간 대기
        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minimumLoadTime)
        {
            _loadingProgress = 0.99f;
            OnSceneLoadProgress?.Invoke(_loadingProgress);
            yield return new WaitForSeconds(minimumLoadTime - elapsedTime);
        }

        // 로딩 완료
        _loadingProgress = 1f;
        OnSceneLoadProgress?.Invoke(_loadingProgress);

        // 씬 활성화
        asyncLoad.allowSceneActivation = true;

        // 씬이 완전히 로드될 때까지 대기
        yield return asyncLoad;

        // 사용하지 않는 리소스 언로드 (비동기)
        AsyncOperation unloadOp = Resources.UnloadUnusedAssets();
        yield return unloadOp;

        //씬 데이터 업데이트
        _currentSceneData = _nextSceneData;
        _sceneType = _currentSceneData.SceneType;
        _nextSceneData = null;


        // 로딩 완료 이벤트 발생
        OnSceneLoadComplete?.Invoke(sceneName);
        OnTransitionComplete?.Invoke();
        
        _isLoading = false;
    }
    #endregion

}
