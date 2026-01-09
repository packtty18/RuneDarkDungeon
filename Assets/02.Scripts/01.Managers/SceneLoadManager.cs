using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



/// 씬 로딩,전환 및 씬 데이터를 관리하는 싱글톤 매니저
/// SceneTransition.cs 컴포넌트 추가 후 사용
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
    private SceneDataSO _currentSceneData; //현재 씬 데이터
    private SceneDataSO _nextSceneData; // 로드할 씬 데이터
    private ESceneType _sceneType; //현재 씬 타입

    [SerializeField]
    private string _loadingSceneName = "LoadingScene";
    private Dictionary<string, object> _sceneData = new Dictionary<string, object>();


    public SceneDataSO StartSceneData;
    public bool IsLoading => _isLoading;
    public float LoadingProgress => _loadingProgress;


    public SceneDataSO NextSceneData => _nextSceneData;
    public SceneDataSO CurrentSceneData => _currentSceneData;
    public ESceneType SceneType => _sceneType; 

    protected override void Awake()
    {
        base.Awake();
        _currentSceneData = StartSceneData;
        _sceneType = _currentSceneData.SceneType;
    }

    #region Public Methods - Scene Loading
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
        _sceneType = ESceneType.Loading;
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

    //로딩 실패 시 이전 씬으로 돌아감
    private void LoadFallbackScene()
    {
        Debug.LogWarning($"폴백 씬으로 이동: {_currentSceneData.SceneName}");
        SceneManager.LoadScene(_currentSceneData.SceneName);
    }
    #endregion

    #region Public Methods - Scene Data

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

    public void RemoveSceneData(string key)
    {
        if (_sceneData.ContainsKey(key))
        {
            _sceneData.Remove(key);
        }
    }

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
