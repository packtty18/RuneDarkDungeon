using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Managers
{
    /// <summary>
    /// 씬 전환 시 페이드 인/아웃 효과를 관리하는 싱글톤 클래스
    /// Singleton class managing fade in/out effects during scene transitions
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class SceneTransition : MonoBehaviour
    {
        #region Singleton
        private static SceneTransition _instance;
        public static SceneTransition Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 씬에서 찾기
                    _instance = FindObjectOfType<SceneTransition>();
                    
                    if (_instance == null)
                    {
                        // 없으면 생성
                        GameObject go = new GameObject("SceneTransition");
                        _instance = go.AddComponent<SceneTransition>();
                    }
                    
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        #endregion

        #region Inspector Fields
        [Header("Fade Settings")]
        [SerializeField] private Image _fadeImage;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private Color _fadeColor = Color.black;
        
        [Header("Canvas Settings")]
        [SerializeField] private int _sortingOrder = 999;
        #endregion

        #region Private Fields
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private bool _isFading = false;
        #endregion

        #region Public Properties
        /// <summary>현재 페이드 효과가 진행 중인지 여부</summary>
        public bool IsFading => _isFading;
        
        /// <summary>페이드 지속 시간</summary>
        public float FadeDuration
        {
            get => _fadeDuration;
            set => _fadeDuration = Mathf.Max(0.1f, value);
        }
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            // 싱글톤 체크
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeComponents();
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
        #endregion

        #region Initialization
        private void InitializeComponents()
        {
            // Canvas 설정
            _canvas = GetComponent<Canvas>();
            if (_canvas == null)
            {
                _canvas = gameObject.AddComponent<Canvas>();
            }
            
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = _sortingOrder;

            // CanvasScaler 추가 (해상도 대응)
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            if (scaler == null)
            {
                scaler = gameObject.AddComponent<CanvasScaler>();
            }
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            // GraphicRaycaster 추가 (상호작용 차단용)
            if (GetComponent<GraphicRaycaster>() == null)
            {
                gameObject.AddComponent<GraphicRaycaster>();
            }

            // CanvasGroup 설정
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            // Fade 이미지 생성
            if (_fadeImage == null)
            {
                CreateFadeImage();
            }

            // 초기 상태: 투명
            SetAlpha(0f);
            _canvasGroup.blocksRaycasts = false;
        }

        private void CreateFadeImage()
        {
            GameObject imageObj = new GameObject("FadeImage");
            imageObj.transform.SetParent(transform, false);

            _fadeImage = imageObj.AddComponent<Image>();
            _fadeImage.color = _fadeColor;

            // 전체 화면 크기로 설정
            RectTransform rectTransform = _fadeImage.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 페이드 아웃 효과 (화면이 어두워짐)
        /// Fade out effect (screen becomes dark)
        /// </summary>
        /// <param name="duration">페이드 지속 시간 (null이면 기본값 사용)</param>
        /// <returns>코루틴</returns>
        public IEnumerator FadeOut(float? duration = null)
        {
            if (_isFading)
            {
                Debug.LogWarning("[SceneTransition] 이미 페이드 효과가 진행 중입니다. Already fading.");
                yield break;
            }

            float fadeDuration = duration ?? _fadeDuration;
            _isFading = true;
            _canvasGroup.blocksRaycasts = true;

            yield return Fade(0f, 1f, fadeDuration);

            _isFading = false;
        }

        /// <summary>
        /// 페이드 인 효과 (화면이 밝아짐)
        /// Fade in effect (screen becomes bright)
        /// </summary>
        /// <param name="duration">페이드 지속 시간 (null이면 기본값 사용)</param>
        /// <returns>코루틴</returns>
        public IEnumerator FadeIn(float? duration = null)
        {
            if (_isFading)
            {
                Debug.LogWarning("[SceneTransition] 이미 페이드 효과가 진행 중입니다. Already fading.");
                yield break;
            }

            float fadeDuration = duration ?? _fadeDuration;
            _isFading = true;

            yield return Fade(1f, 0f, fadeDuration);

            _canvasGroup.blocksRaycasts = false;
            _isFading = false;
        }

        /// <summary>
        /// 즉시 페이드 아웃 (애니메이션 없음)
        /// Instant fade out (no animation)
        /// </summary>
        public void FadeOutImmediate()
        {
            SetAlpha(1f);
            _canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// 즉시 페이드 인 (애니메이션 없음)
        /// Instant fade in (no animation)
        /// </summary>
        public void FadeInImmediate()
        {
            SetAlpha(0f);
            _canvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// 페이드 색상을 변경합니다
        /// Changes the fade color
        /// </summary>
        /// <param name="color">새로운 색상</param>
        public void SetFadeColor(Color color)
        {
            _fadeColor = color;
            if (_fadeImage != null)
            {
                _fadeImage.color = color;
            }
        }
        #endregion

        #region Private Methods
        private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime; // Time.timeScale의 영향을 받지 않음
                float normalizedTime = elapsed / duration;
                float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);
                
                SetAlpha(currentAlpha);
                
                yield return null;
            }

            SetAlpha(endAlpha);
        }

        private void SetAlpha(float alpha)
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = alpha;
            }
        }
        #endregion

        #region Static Helper Methods
        /// <summary>
        /// 씬 전환용 페이드 효과를 실행합니다 (씬 매니저와 함께 사용)
        /// Executes fade effect for scene transition (used with SceneManager)
        /// </summary>
        /// <param name="fadeOutDuration">페이드 아웃 지속 시간</param>
        /// <param name="fadeInDuration">페이드 인 지속 시간</param>
        /// <returns>코루틴</returns>
        public static IEnumerator PerformTransition(float fadeOutDuration = 0.5f, float fadeInDuration = 0.5f)
        {
            if (Instance == null)
            {
                Debug.LogError("[SceneTransition] Instance가 null입니다. Instance is null.");
                yield break;
            }

            yield return Instance.FadeOut(fadeOutDuration);
            yield return Instance.FadeIn(fadeInDuration);
        }
        #endregion
    }
}