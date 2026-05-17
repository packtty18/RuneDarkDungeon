using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkillCaster : PoolSpawner
{
    private PlayerStateController _stateController;
    private PlayerAttack _playerAttack;
    private PlayerAnimator _animator;
    private ESkillSlot _currentSlot;

    [SerializeField]
    private HitboxController _hitboxController;
    private IEquipment _equipment;
    private ItemData _currentItem;

    public event Action<Dictionary<ESkillSlot, float>, Dictionary<ESkillSlot, float>> OnCoolTimeChanged;

    // 쿨다운 관리.
    private Dictionary<ESkillSlot, float> _cooldowns = new Dictionary<ESkillSlot, float>()
    {
        { ESkillSlot.Q, 0f },
        { ESkillSlot.E, 0f },
        { ESkillSlot.R, 0f }
    };

    // 쿨타임 관리.
    private Dictionary<ESkillSlot, float> _coolTimes = new Dictionary<ESkillSlot, float>()
    {
        { ESkillSlot.Q, 0f },
        { ESkillSlot.E, 0f },
        { ESkillSlot.R, 0f }
    };

    private void Awake()
    {
        _stateController = GetComponent<PlayerStateController>();
        _playerAttack = GetComponent<PlayerAttack>();
        _animator = GetComponent<PlayerAnimator>();
    }

    public void Initialize(IEquipment equipment)
    {
        _equipment = equipment;
    }

    private void Update()
    {
        // 쿨다운 감소.
        UpdateCooldowns();

        if (!_stateController.CanReceiveSkillInput()) return;

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

        if (!_equipment.TryGetItem(slot, out var item)) return;
        _currentItem = item;
        _currentSlot = slot;
        
        // 2. 쿨다운 체크.
        if (_cooldowns[slot] > 0f)
        {
            //Debug.Log($"[Skill] {slot} 쿨다운 중: {_cooldowns[slot]:F1}초 남음");
            return;
        }

        // 3. 스킬 시작.
        CastSkill();

        // 4. 쿨다운 시작.
        CoolDownStart();
    }

    private void CoolDownStart()
    {
        if (_coolTimes[_currentSlot] == 0f)
        {
            _coolTimes[_currentSlot] = _currentItem.GetCoolTime();
        }
        _cooldowns[_currentSlot] = _coolTimes[_currentSlot];
    }

    private void CastSkill()
    {
        OnSkillStart();
        
        AnimationClip clip = _currentItem.GetClip();
        
        _animator.PlaySkill(clip);
    }


    private void UpdateCooldowns()
    {
        foreach (var slot in _cooldowns.Keys.ToList())
        {
            if (_cooldowns[slot] > 0f)
            {
                OnCoolTimeChanged?.Invoke(_coolTimes, _cooldowns);
                _cooldowns[slot] -= Time.deltaTime;
                if (_cooldowns[slot] < 0f)
                    _cooldowns[slot] = 0f;
            }
        }
        
    }

    private void OnSkillStart()
    {
        _stateController.SetActionState(EActionState.Skill);

        _playerAttack.OnSkillInterrupt();

        //Debug.Log($"[Skill] 스킬 실행");
    }

    public void OnSkillEffect()
    {
        GameObject skillObject = GetFromPool(_currentItem.Skill);
        skillObject.transform.position = transform.position;

        if (!skillObject.TryGetComponent(out SkillBase skill)) return;
        skill.OnUse(gameObject, _currentItem.Grade);
    }

    //스킬 사용 종료 타이밍에 맞춰 애니메이션 이벤트로 호출.
    public void OnSkillEnd()
    {
        _stateController.SetActionState(EActionState.None);

        _playerAttack.OnSkillComplete();

        _currentSlot = ESkillSlot.None;

        //Debug.Log($"[Skill] 스킬 종료");
    }
}