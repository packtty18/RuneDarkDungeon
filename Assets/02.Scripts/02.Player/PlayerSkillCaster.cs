using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkillCaster : MonoBehaviour
{
    private PlayerStateMachine _stateMachine;
    private PlayerAttack _playerAttack;
    private PlayerMove _playerMove;
    
    // 쿨다운 관리
    private Dictionary<ESkillSlot, float> _cooldowns = new Dictionary<ESkillSlot, float>()
    {
        { ESkillSlot.Q, 0f },
        { ESkillSlot.E, 0f },
        { ESkillSlot.R, 0f }
    };

    private void Awake()
    {
        _stateMachine = GetComponent<PlayerStateMachine>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        // 쿨다운 감소.
        UpdateCooldowns();

        if (_stateMachine.CurrentActionState == EActionState.Skill) return;

        if (InputManager.Instance.GetKeyDown(EGameKeyType.QSkill))
            TryCastSkill(ESkillSlot.Q);

        if (InputManager.Instance.GetKeyDown(EGameKeyType.ESkill))
            TryCastSkill(ESkillSlot.E);

        if (InputManager.Instance.GetKeyDown(EGameKeyType.RSkill))
            TryCastSkill(ESkillSlot.R);
    }

    private void TryCastSkill(ESkillSlot slot)
    {
        // 1. 룬이 장착되어 있는지 확인.


        // 2. 쿨다운 체크.
        if (_cooldowns[slot] > 0f)
        {
            Debug.Log($"[Skill] {slot} 쿨다운 중: {_cooldowns[slot]:F1}초 남음");
            return;
        }

        // 4. 스킬 실행.
        CastSkill();

        // 5. 쿨다운 시작.
        _cooldowns[slot] = 5f;
    }

    private void CastSkill()
    {
        OnSkillStart();

        StartCoroutine(SkillCoroutine());
    }

    private IEnumerator SkillCoroutine()
    {
        yield return new WaitForSeconds(3f);


        OnSkillEnd();
    }

    private void UpdateCooldowns()
    {
        foreach (var slot in _cooldowns.Keys.ToList())
        {
            if (_cooldowns[slot] > 0f)
            {
                _cooldowns[slot] -= Time.deltaTime;
                if (_cooldowns[slot] < 0f)
                    _cooldowns[slot] = 0f;
            }
        }
    }

    private void OnSkillStart()
    {
        _stateMachine.SetActionState(EActionState.Skill);

        _playerAttack.OnSkillInterrupt();

        Debug.Log($"[Skill] 스킬 실행");
    }

    //스킬 사용 종료 타이밍에 맞춰 애니메이션 이벤트로 호출.
    public void OnSkillEnd()
    {
        _stateMachine.SetActionState(EActionState.None);

        _playerAttack.OnSkillComplete();

        Debug.Log($"[Skill] 스킬 종료");
    }
}