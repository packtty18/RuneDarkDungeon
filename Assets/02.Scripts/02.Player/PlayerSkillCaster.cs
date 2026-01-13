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
    private PlayerAnimator _animator;
    private ESkillSlot _currentSlot;

    [SerializeField] private Equipment _equipment;
    public ItemDatabaseSO DB;

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
        _animator = GetComponent<PlayerAnimator>();

        SetItemInfo(_equipment.Items.Values);
    }

    private void SetItemInfo(IEnumerable<ItemData> items)
    {
        foreach (var item in items)
        {
            if (item == null) continue;
            ItemSO info = DB.GetItemInfo(item);
            item.SetInfo(info);
        }
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

        _currentSlot = slot;

        // 2. 쿨다운 체크.
        if (_cooldowns[slot] > 0f)
        {
            Debug.Log($"[Skill] {slot} 쿨다운 중: {_cooldowns[slot]:F1}초 남음");
            return;
        }

        // 3. 스킬 시작.
        CastSkill();

        // 4. 쿨다운 시작.
        _cooldowns[slot] = _equipment.GetCoolTime(_currentSlot);
    }

    private void CastSkill()
    {
        OnSkillStart();

        AnimationClip clip = _equipment.GetClip(_currentSlot);
        
        _animator.PlaySkill(clip);
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

    public void OnSkillEffect()
    {
        float coolTime = 0;
        _equipment.UseItem(gameObject, _currentSlot, out coolTime);
    }

    //스킬 사용 종료 타이밍에 맞춰 애니메이션 이벤트로 호출.
    public void OnSkillEnd()
    {
        _stateMachine.SetActionState(EActionState.None);

        _playerAttack.OnSkillComplete();

        _currentSlot = ESkillSlot.None;

        Debug.Log($"[Skill] 스킬 종료");
    }
}