using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class GameFlowManager : LocalSingleton<GameFlowManager>
{
    [Header("UI")]
    [SerializeField]
    private UI_Popup _pauseUI;
    [SerializeField]
    private RewardPresenter _resultUI;
    [SerializeField]
    private UI_BasicAnimation _playerHUD;
    [SerializeField]
    private UI_BasicAnimation _fadeBlack;

    [Header("입력")]
    [SerializeField]
    private EGameKeyType _pauseKey;

    [Header("타이밍")]
    [SerializeField]
    private float _defaultFadeTime = 0.5f;
    [SerializeField]
    private float _deathFadeTime = 2.0f;
    [SerializeField]
    private float _gameOverDelay = 2.0f;
    [SerializeField]
    private float _clearDelay = 2.0f;

    [Header("컷씬")]
    [SerializeField]
    private PlayableDirector _startSceneDirector;
    [SerializeField]
    private PlayableDirector _deathSceneDirector;
    [SerializeField]
    private PlayableDirector _clearSceneDirector;

    private Coroutine _startSceneSkipCoroutine;

    private bool _isPaused = false;
    private bool _isPlayingCutScene = false;

    public bool IsPaused => _isPaused;
    public bool IsPlayingCutScene => _isPlayingCutScene;


    #region Life Cycle
    protected override void Awake()
    {
        base.Awake();
        Initialize();
        EventSubscribe();
        _playerHUD.Hide();
    }

    private void Start()
    {
        PlayStartScene();
    }

    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(_pauseKey))
        {
            TogglePause();
        }
    }

    protected override void OnDestroy()
    {
        Resume();
        EventUnsubscribe();
        base.OnDestroy();
    }

    #endregion

    #region Init
    private void Initialize()
    {
        Time.timeScale = 1.0f;
    }

    #endregion

    #region Game Pause

    public void TogglePause()
    {
        if (_isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (_isPaused) return;

        _isPaused = true;
        Time.timeScale = 0f;
        _pauseUI.Show();

        BattleManager.Instance?.NotifyGamePause();
    }

    public void Resume()
    {
        if (!_isPaused) return;

        _isPaused = false;
        Time.timeScale = 1f;
        _pauseUI.Hide();

        BattleManager.Instance?.NotifyGameResume();
    }

    #endregion

    #region Coroutine
    private IEnumerator StartCutSceneSkip()
    {
        while (true)
        {
            if (InputManager.Instance.GetKeyDown(EGameKeyType.Enter))
            {
                break;
            }
            yield return null;
        }
        _startSceneDirector.Stop();
    }

    private IEnumerator ClearUIPopupDelay()
    {
        yield return new WaitForSeconds(_clearDelay);
        _resultUI.ShowVictory();
    }
    private IEnumerator GameOverUIPopupDelay()
    {
        yield return new WaitForSeconds(_gameOverDelay);
        _fadeBlack?.FadeIn(_deathFadeTime, DG.Tweening.Ease.OutElastic).OnComplete(() =>
        {
            _resultUI.ShowDefeat();
        });
    }

    #endregion

    #region Play Cut Scene & End Event

    private void PlayStartScene()
    {
        _startSceneDirector.Play();
        _isPlayingCutScene = true;
        _startSceneSkipCoroutine = StartCoroutine(StartCutSceneSkip());
    }
    public void OnGameOver()
    {
        _playerHUD.FadeOut(_defaultFadeTime);
        _deathSceneDirector.Play();
        StartCoroutine(GameOverUIPopupDelay());
    }

    [Button]
    public void OnClear()
    {
        _playerHUD.FadeOut(_defaultFadeTime);
        _clearSceneDirector.Play();
        _isPlayingCutScene = true;
        StartCoroutine(ClearUIPopupDelay());
    }

    private void OnClearTimelineEnd(PlayableDirector director)
    {
        _isPlayingCutScene = false;
    }

    private void OnDeathTimelineEnd(PlayableDirector director)
    {
        _isPlayingCutScene = false;
    }

    private void OnStartTimelineEnd(PlayableDirector director)
    {
        _isPlayingCutScene = false;
        StopCoroutine(_startSceneSkipCoroutine);
        _fadeBlack?.Show();
        _fadeBlack?.FadeOut(_defaultFadeTime);
        _playerHUD.FadeIn(_defaultFadeTime);
    }

    #endregion

    #region Event Subscribe
    private void EventSubscribe()
    {
        _startSceneDirector.stopped += OnStartTimelineEnd;
        _deathSceneDirector.stopped += OnDeathTimelineEnd;
        _clearSceneDirector.stopped += OnClearTimelineEnd;
    }

    private void EventUnsubscribe()
    {
        _startSceneDirector.stopped -= OnStartTimelineEnd;
        _deathSceneDirector.stopped -= OnDeathTimelineEnd;
        _clearSceneDirector.stopped -= OnClearTimelineEnd;
    }

    #endregion
}
