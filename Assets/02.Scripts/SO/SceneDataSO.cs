using UnityEngine;

/// <summary>
/// 씬 정보를 저장하는 ScriptableObject
/// ScriptableObject for storing scene information
/// </summary>
[CreateAssetMenu(fileName = "New Scene Data", menuName = "Game/Scene Data", order = 1)]
public class SceneDataSO : ScriptableObject
{
    #region Inspector Fields
    [Header("Scene Info")]
    [Tooltip("씬 이름 (Build Settings의 씬 이름과 일치해야 함)")]
    [SerializeField] private string _sceneName;

    [Tooltip("씬 표시 이름 (UI에 표시될 이름)")]
    [SerializeField] private string _displayName;

    [Tooltip("씬 설명")]
    [TextArea(3, 5)]
    [SerializeField] private string _description;

    [Tooltip("최소 로딩 시간 (초) - 너무 빠른 로딩 방지")]
    [SerializeField] private float _minimumLoadTime = 1f;

    [Header("Scene Properties")]
    [Tooltip("씬 타입")]
    [SerializeField] private ESceneType _sceneType = ESceneType.Gameplay;

    [Tooltip("이 씬을 로드할 때 표시할 로딩 팁")]
    [SerializeField] private string[] _loadingTips;

    #endregion

    #region Public Properties
    /// <summary>씬 이름</summary>
    public string SceneName => _sceneName;

    /// <summary>표시 이름</summary>
    public string DisplayName => string.IsNullOrEmpty(_displayName) ? _sceneName : _displayName;

    /// <summary>씬 설명</summary>
    public string Description => _description;


    /// <summary>최소 로딩 시간</summary>
    public float MinimumLoadTime => _minimumLoadTime;


    /// <summary>씬 타입</summary>
    public ESceneType SceneType => _sceneType;

    /// <summary>로딩 팁</summary>
    public string[] LoadingTips => _loadingTips;

    #endregion

    #region Validation
    private void OnValidate()
    {
        // 씬 이름이 비어있으면 경고
        if (string.IsNullOrEmpty(_sceneName))
        {
            Debug.LogWarning($"[SceneData] {name}: Scene name is empty!");
        }

        // 최소 로딩 시간 검증
        if (_minimumLoadTime < 0f)
        {
            _minimumLoadTime = 0f;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// 이 씬 데이터를 사용하여 씬을 로드합니다
    /// Loads the scene using this scene data
    /// </summary>
    public void LoadScene()
    {
        if (string.IsNullOrEmpty(_sceneName))
        {
            Debug.LogError($"[SceneData] {name}: Cannot load scene - scene name is empty!");
            return;
        }

        var sceneLoadManager = SceneLoadManager.Instance;

        // 메인 씬 로드
        sceneLoadManager.BeginSceneLoad(this);
    }

    /// <summary>
    /// 씬 이름을 반환합니다 (ToString 오버라이드)
    /// Returns scene name (ToString override)
    /// </summary>
    public override string ToString()
    {
        return $"SceneData: {DisplayName} ({_sceneName})";
    }
    #endregion
}
