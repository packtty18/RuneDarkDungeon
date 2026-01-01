using GameCore.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.Managers
{
    /// <summary>
    /// 씬 로딩 및 전환을 관리하는 싱글톤 매니저
    /// Singleton manager for scene loading and transitions
    /// </summary>
    public class SceneManager : GlobalSingleton<SceneManager>
    {
        #region Events
        /// <summary>씬 로딩 시작 시 호출되는 이벤트</summary>
        public event Action<string> OnSceneLoadStart;
        
        /// <summary>씬 로딩 진행률 업데이트 이벤트 (0.0 ~ 1.0)</summary>
        public event Action<float> OnSceneLoadProgress;
        
        /// <summary>씬 로딩 완료 시 호출되는 이벤트</summary>
        public event Action<string> OnSceneLoadComplete;
        
        /// <summary>씬 언로딩 시작 시 호출되는 이벤트</summary>
        public event Action<string> OnSceneUnloadStart;
        
        /// <summary>씬 언로딩 완료 시 호출되는 이벤트</summary>
        public event Action<string> OnSceneUnloadComplete;
        
        /// <summary>씬 전환 시작 시 호출되는 이벤트</summary>
        public event Action OnTransitionStart;
        
        /// <summary>씬 전환 완료 시 호출되는 이벤트</summary>
        public event Action OnTransitionComplete;
        #endregion


        private string _currentSceneName;
        private bool _isLoading = false;
        private float _loadingProgress = 0f;
        private SceneDataSO _currentSceneData;

        [SerializeField]
        private string _loadingSceneName = "LoadingScene";
        private Dictionary<string, object> _sceneData = new Dictionary<string, object>();

        public bool IsLoading => _isLoading;
        public float LoadingProgress => _loadingProgress;
        public string CurrentSceneName => _currentSceneName;

        public SceneDataSO CurrentSceneData => _currentSceneData;

        #region Public Methods - Scene Loading
        /// <summary>
        /// 씬을 비동기로 로드합니다 (기존 씬은 언로드)
        /// Asynchronously loads a scene (unloads previous scene)
        /// </summary>
        /// <param name="sceneName">로드할 씬 이름</param>
        /// <param name="useTransition">페이드 전환 효과 사용 여부</param>
        /// <param name="minimumLoadTime">최소 로딩 시간 (초)</param>
        public void LoadLoading(SceneDataSO dataSO)
        {
            _currentSceneData = dataSO;

            if (_isLoading)
            {
                Debug.LogWarning($"[SceneManager] 이미 로딩 중입니다. Already loading: {_currentSceneData.name}");
                return;
            }
            //로딩씬 전환
            UnityEngine.SceneManagement.SceneManager.LoadScene(_loadingSceneName);
            _currentSceneName = _loadingSceneName;
        }

        public void LoadScene()
        {
            StartCoroutine(LoadSceneAsync(_currentSceneData.SceneName, _currentSceneData.UseTransition, _currentSceneData.MinimumLoadTime));
        }

        /// <summary>
        /// 현재 씬을 다시 로드합니다
        /// Reloads the current scene
        /// </summary>
        /// <param name="useTransition">페이드 전환 효과 사용 여부</param>
        public void ReloadCurrentScene(bool useTransition = true)
        {
            LoadScene();
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
                    Debug.LogError($"[SceneManager] 데이터 타입 변환 실패. Failed to cast data: {key}");
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


        private IEnumerator LoadSceneAsync(string sceneName, bool useTransition = false, float minimumLoadTime = 0)
        {
            _isLoading = true;
            _loadingProgress = 0f;
            float startTime = Time.time;

            // 전환 시작 이벤트 발생
            OnTransitionStart?.Invoke();
            OnSceneLoadStart?.Invoke(sceneName);

            /*// 페이드 아웃 효과
            if (useTransition && SceneTransition.Instance != null)
            {
                yield return StartCoroutine(SceneTransition.Instance.FadeOut());
            }*/

            // 비동기 씬 로드 시작
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            
            if (asyncLoad == null)
            {
                Debug.LogError($"[SceneManager] 씬 로드 실패. Failed to load scene: {sceneName}");
                _isLoading = false;
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

            // 가비지 컬렉션 실행 (메모리 최적화)
            Resources.UnloadUnusedAssets();
            System.GC.Collect();

            _currentSceneName = sceneName;

            /*// 페이드 인 효과
            if (useTransition && SceneTransition.Instance != null)
            {
                yield return StartCoroutine(SceneTransition.Instance.FadeIn());
            }*/

            // 로딩 완료 이벤트 발생
            OnSceneLoadComplete?.Invoke(sceneName);
            OnTransitionComplete?.Invoke();

            _isLoading = false;
        }

  
    }
}