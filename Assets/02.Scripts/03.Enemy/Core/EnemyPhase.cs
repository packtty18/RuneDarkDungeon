using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhase : MonoBehaviour
{
    [Title("Phase 설정")]
    [SerializeField, InfoBox("Phase 데이터를 순서대로 등록하세요")]
    private List<PhaseDataSO> _phaseDataList = new List<PhaseDataSO>();

    [ShowInInspector, ReadOnly]
    private int _currentPhaseIndex = 0;

    [ShowInInspector, ReadOnly]
    private PhaseDataSO _currentPhase;

    private EnemyController _controller;

    public int CurrentPhaseIndex => _currentPhaseIndex;
    public PhaseDataSO CurrentPhase => _currentPhase;
    public bool HasPhases => _phaseDataList != null && _phaseDataList.Count > 0;

    private void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }

    public void Init()
    {
        if (!HasPhases)
        {
            return;
        }

        // Phase 인덱스 순서대로 정렬
        _phaseDataList.Sort((a, b) => a.PhaseIndex.CompareTo(b.PhaseIndex));

        Reset();
    }

    /// <summary>
    /// Phase 시스템 리셋
    /// </summary>
    [Button("Phase 리셋")]
    public void Reset()
    {
        _currentPhaseIndex = 0;

        if (HasPhases)
        {
            _currentPhase = _phaseDataList[0];
            EnterPhase(_currentPhase);
        }
    }

    public void CheckPhaseTransition()
    {
        if (!HasPhases || _controller == null)
        {
            return;
        }

        // 다음 Phase가 있는지 확인
        int nextPhaseIndex = _currentPhaseIndex + 1;
        if (nextPhaseIndex >= _phaseDataList.Count)
        {
            return; // 더 이상 Phase가 없음
        }

        PhaseDataSO nextPhase = _phaseDataList[nextPhaseIndex];

        // 전환 조건 체크
        if (nextPhase.CheckTransition(_controller))
        {
            TransitionToPhase(nextPhaseIndex);
        }
    }

    private void TransitionToPhase(int phaseIndex)
    {
        if (phaseIndex < 0 || phaseIndex >= _phaseDataList.Count)
        {
            Debug.LogWarning($"[PhaseController] 유효하지 않은 Phase Index: {phaseIndex}");
            return;
        }

        if (phaseIndex <= _currentPhaseIndex)
        {
            Debug.LogWarning($"[PhaseController] 이전 Phase로 전환 불가: {phaseIndex}");
            return;
        }

        _currentPhaseIndex = phaseIndex;
        _currentPhase = _phaseDataList[phaseIndex];

        EnterPhase(_currentPhase);
    }

    private void EnterPhase(PhaseDataSO phase) 
    { 

        foreach (var buff in phase.BuffsToApply)
        {
            _controller.Buff.ApplyBuff(buff);
        }

        // 공격 패턴 변경
        if (phase.ChangeAttackPattern)
        {
            HandleAttackPatternChange(phase);
        }
    }

    private void HandleAttackPatternChange(PhaseDataSO phase)
    {
        if (_controller.Stat.EnemyType == EEnemyType.Boss)
        {
            BossAttack bossAttack = _controller.Attack as BossAttack;
            if (bossAttack == null) return;

            switch (phase.PhaseIndex)
            {
                case 1: // Phase 2
                    bossAttack.Phase2Strategy();
                    break;
                case 2: // Phase 3
                    bossAttack.Phase3Strategy();
                    break;
            }
        }
    }

    public bool IsPhase(int phaseIndex)
    {
        return _currentPhaseIndex == phaseIndex;
    }

#if UNITY_EDITOR
    [Title("에디터 전용")]
    [Button("다음 Phase로 강제 전환"), DisableInEditorMode]
    private void ForceNextPhase()
    {
        if (_currentPhaseIndex + 1 < _phaseDataList.Count)
        {
            TransitionToPhase(_currentPhaseIndex + 1);
        }
    }
#endif
}