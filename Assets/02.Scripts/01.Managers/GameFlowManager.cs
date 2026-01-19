using UnityEngine;

public class GameFlowManager : LocalSingleton<GameFlowManager>
{
    [SerializeField]
    private UI_Popup _pauseUI;
    [SerializeField]
    private EGameKeyType _pauseKey;

    private bool _isPaused = false;
    public bool IsPaused => _isPaused;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }
    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(_pauseKey))
        {
            TogglePause();
        }
    }

    private void Initialize()
    {
        Time.timeScale = 1.0f;
    }

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

    protected override void OnDestroy()
    {
        Resume();
        base.OnDestroy();
    }
}
