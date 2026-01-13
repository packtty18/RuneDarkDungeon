using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public enum EBattleState
{
    None,
    Preparing,          // 던전 준비
    InProgress,         // 전투 진행 중
    Paused,             // 컷신 / UI
    WaitingNextStage,   // 문 파괴 대기
    Victory,            // 클리어
    Defeat              // 패배
}

public class BattleManager : LocalSingleton<BattleManager>
{
    [Header("References")]
    [SerializeField] private EnemySpawnManager[] _spawnManagers;

    [Header("Battle Events")]
    public UnityEvent OnBattleStart;
    public UnityEvent OnBattleWin;
    public UnityEvent OnBattleLose;

    public SafeEvent<EBattleState> OnBattleStateChanged = new();

    public Transform PlayerTransform;
    private int _currentIndex;
    [SerializeField] private EBattleState _state;
    public EBattleState State => _state;

    private bool _isStageRunning;

    private void Start()
    {
        SetState(EBattleState.Preparing);
    }

    #region State Control

    [Button]
    private void SetState(EBattleState newState)
    {
        if (_state == newState)
            return;

        _state = newState;
        Debug.Log($"[BattleManager] State Changed → {_state}");

        OnBattleStateChanged.Invoke(_state);

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

    #endregion

    #region State Handlers

    private void HandlePreparing()
    {
        _currentIndex = 0;
        _isStageRunning = false;

        OnBattleStart?.Invoke();
        SetState(EBattleState.InProgress);
    }

    private void HandleInProgress()
    {
        // 이미 진행 중이면 재시작 금지 (Pause → Resume 대응)
        if (_isStageRunning)
        {
            Debug.Log("[BattleManager] Resume Battle");
            return;
        }

        if (_currentIndex >= _spawnManagers.Length)
        {
            SetState(EBattleState.Victory);
            return;
        }

        Debug.Log($"[BattleManager] Start Stage {_currentIndex}");

        _isStageRunning = true;

        EnemySpawnManager manager = _spawnManagers[_currentIndex];
        manager.OnAllPhaseCompleted.Subscribe(HandleCurrentStageCleared);
        manager.PlayCurrentPhase();
    }

    private void HandleCurrentStageCleared()
    {
        EnemySpawnManager manager = _spawnManagers[_currentIndex];
        manager.OnAllPhaseCompleted.Unsubscribe(HandleCurrentStageCleared);

        _isStageRunning = false;   // ⭐ 중요
        _currentIndex++;

        SetState(EBattleState.WaitingNextStage);
    }

    #endregion

    #region External Notifications

    //문을 부순다면 다음 스테이지 시작됨
    [Button]
    public void NotifyDoorDestroyed()
    {
        if (_state != EBattleState.WaitingNextStage)
            return;

        SetState(EBattleState.InProgress);
    }

    //플레이어 사망
    [Button]
    public void NotifyPlayerDead()
    {
        SetState(EBattleState.Defeat);
    }

    //UI나 연출적인 부분으로 인해 적들이 멈춰야하는경우
    [Button]
    public void PauseBattle()
    {
        SetState(EBattleState.Paused);
    }

    //다시 재시작
    [Button]
    public void ResumeBattle()
    {
        SetState(EBattleState.InProgress);
    }

    #endregion
}
