using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public enum EBattleState
{
    None,
    Preparing,
    InProgress,
    Pause,
    WaitingNextStage,
    Victory,
    Defeat
}

public class BattleManager : LocalSingleton<BattleManager>
{
    [SerializeField] public Transform PlayerTransform;

    [Header("Stage")]
    [SerializeField] private EnemySpawnManager[] _spawnManagers;

    [Header("Events")]
    public UnityEvent OnBattleStart;
    public UnityEvent OnBattleWin;
    public UnityEvent OnBattleLose;

    public SafeEvent<EBattleState> OnBattleStateChanged = new();

    public EBattleState State => _state;

    [SerializeField]private EBattleState _state;
    private int _currentIndex;
    private bool _isStageRunning;

    private void Start()
    {
        SetState(EBattleState.Preparing);
    }

    [Button]
    private void SetState(EBattleState newState)
    {
        if (_state == newState)
            return;

        _state = newState;
        //Debug.Log($"[BattleManager] State → {_state}");
        OnBattleStateChanged?.Invoke(_state);

        switch (_state)
        {
            case EBattleState.Preparing:
                HandlePreparing();
                break;

            case EBattleState.InProgress:
                HandleInProgress();
                break;

            case EBattleState.Victory:
                OnBattleWin?.Invoke();
                break;

            case EBattleState.Defeat:
                OnBattleLose?.Invoke();
                break;
        }
    }

    private void HandlePreparing()
    {
        _currentIndex = 0;
        _isStageRunning = false;
        OnBattleStart?.Invoke();
        HitBox.ResetId();
        ActiveCurrentSpawnManager();

        SetState(EBattleState.WaitingNextStage);
    }

    private void HandleInProgress()
    {
        if (_isStageRunning)
        {
            //Debug.Log("[BattleManager] Resume Stage");
            return;
        }

        if (_currentIndex >= _spawnManagers.Length)
        {
            SetState(EBattleState.Victory);
            return;
        }

        _isStageRunning = true;
        
    }

    private void ActiveCurrentSpawnManager()
    {
        if(_currentIndex >=_spawnManagers.Length)
        {
            OnBattleWin.Invoke();
            return;
        }

        EnemySpawnManager manager = _spawnManagers[_currentIndex];
        manager.OnAllPhaseCompleted.Subscribe(HandleStageCleared);
        manager.SpawnCurrentPhase();
    }

    private void HandleStageCleared()
    {
        EnemySpawnManager manager = _spawnManagers[_currentIndex];
        manager.OnAllPhaseCompleted.Unsubscribe(HandleStageCleared);

        _isStageRunning = false;
        _currentIndex++;

        SetState(EBattleState.WaitingNextStage);
        ActiveCurrentSpawnManager();
    }

    [Button]
    public void NotifyActiveStage()
    {
        if (_state == EBattleState.WaitingNextStage)
            SetState(EBattleState.InProgress);
    }

    [Button]
    public void NotifyPlayerDead()
    {
        SetState(EBattleState.Defeat);
    }

    [Button]
    public void NotifyGamePause()
    {
        
    }

    public void NotifyGameResume()
    {
        
    }
}
