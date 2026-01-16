using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider _progressBar;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private TextMeshProUGUI _tipText;

    private float _targetProgress = 0f;

    private void Start()
    {
        Initialized();

        var sceneLoadManager = SceneLoadManager.Instance;
        sceneLoadManager.OnSceneLoadProgress += SetProgress;
        sceneLoadManager.LoadTargetScene();
        SetLoadingTips();
    }

    private void Initialized()
    {
        _progressBar.value = 0f;
        _progressText.text = $"Loading... 0%";
    }
    public void SetProgress(float progress)
    {
        _targetProgress = Mathf.Clamp01(progress);
        if (_progressBar != null)
        {
            _progressBar.DOKill();
            _progressBar.DOValue(_targetProgress, 0.1f);
        }   
        if (_progressText != null)
            _progressText.text = $"Loading... {Mathf.RoundToInt(_targetProgress * 100f)}%";
    }

    public void SetLoadingTips()
    {
        SceneDataSO sceneData = SceneLoadManager.Instance.NextSceneData;

        if (sceneData == null)
        {
            return;
        }

        string[] tips = sceneData.LoadingTips;

        if (tips == null || tips.Length <= 0 )
        {
            return;
        }
        _tipText.text = tips[Random.Range(0, tips.Length)];
    }

    private void OnDestroy()
    {
        // DOTween 정리
        if (_progressBar != null)
        {
            _progressBar.DOKill();
        }
        if (SceneLoadManager.Instance != null)
        {
            SceneLoadManager.Instance.OnSceneLoadProgress -= SetProgress;
        }
    }
}
