using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

//히트박스 컨트롤러 조절 및 적의 공격체 생성
//이를 상속하여 적 종류별 공격 전략을 추가 => 각 적별로 발사체나 마법을 설정
public class EnemyAttack : MonoBehaviour
{
    [Title("참조")]
    [SerializeField] private HitboxController _hitboxController;

    private readonly Dictionary<int, IAttackStrategy> _strategies = new();
    private IAttackStrategy _current;

    [Title("일단은 테스트")]
    [SerializeField] private Transform _testProjPos;
    [SerializeField] private GameObject _testProj;

    public int StrategyCount => _strategies.Count;
    public bool IsAttacking = false; //State의 판단기준

    public void Init()
    {
        IsAttacking = false;

        //프로토타입의 전략 수립
        RegisterStrategy(0, new ProtoMeleeAttack(_hitboxController));   //기본공격1
        RegisterStrategy(1, new ProtoMeleeAttack(_hitboxController));   //기본공격2
        RegisterStrategy(2, new ProtoMeleeAttack(_hitboxController));   //콤보공격
        //RegisterStrategy(3, new ProtoArrowAttack(_testProj, _testProjPos));   //화살공격
        //RegisterStrategy(4, new ProtoMagicAttack(_testProj, _testProjPos));   //마법공격
        //RegisterStrategy(5, new ProtoMagicAttack(_testProj, _testProjPos));   //버프
    }

    private void RegisterStrategy(int attackID, IAttackStrategy strategy)
    {
        if (_strategies.ContainsKey(attackID))
        {
            Debug.LogWarning($"[EnemyAttack] AttackID {attackID} already registered.");
            return;
        }

        _strategies.Add(attackID, strategy);
    }

    public bool RequestAttack(int attackID)
    {
        if (!_strategies.TryGetValue(attackID, out var strategy))
        {
            Debug.LogWarning($"[EnemyAttack] No strategy for AttackID {attackID}");
            return false;
        }

        _current = strategy;
        IsAttacking = true;
        Debug.Log($"[EnemyAttack] Request Attack : {attackID}");
        return true;
    }

    //공격 취소
    public void CancelAttack()
    {
        if(!IsAttacking)
        {
            return;
        }

        OnAttackEnd();
    }

    #region Animation Events
    //실질적인 공격은 애니메이션 이벤트를 통해 실행하며 각각 다른 방식으로 구현

    //근접 : 히트박스 활성화
    //궁수 : 공격준비
    //마법사 : 캐스팅
    public void OnBeginAttack()
    {
        _current?.BeginAttack();
    }

    //근접 : 없음
    //궁수 : 발사체 생성
    //마법사 : 마법 생성
    public void OnLoopEnd()
    {
        _current?.OnLoopEnd();
    }

    //근접 : 히트박스 비활성화 후 공격종료
    //궁수 : 공격 종료
    //마법사 : 공격 종료
    public void OnAttackEnd()
    {
        _current?.EndAttack();
    }

    //공격 애니메이션 종료
    public void OnAttackComplete()
    {
        _current = null;
        IsAttacking = false;
    }
    #endregion
}
