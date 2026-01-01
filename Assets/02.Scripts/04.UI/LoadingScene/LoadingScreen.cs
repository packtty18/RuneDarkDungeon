using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameCore.Managers
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private TextMeshProUGUI _tipText;

        private float _targetProgress = 0f;

        private void Start()
        {
            var sceneManager = SceneManager.Instance;
            sceneManager.OnSceneLoadProgress += SetProgress;
            sceneManager.LoadScene();
            SetLoadingTips();
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
                _progressText.text = $"{Mathf.RoundToInt(_targetProgress * 100f)}%";
        }

        public void SetLoadingTips()
        {
            string[] array = SceneManager.Instance.CurrentSceneData.LoadingTips;
            _tipText.text = array[Random.Range(0, array.Length)];
        }

        private void OnDestroy()
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.OnSceneLoadProgress -= SetProgress;
            }
        }
    }
}