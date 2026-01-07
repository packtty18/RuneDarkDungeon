using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

//히트박스 컨트롤러 조절 및 적의 공격체 생성
//이를 상속하여 적 종류별 공격 전략을 추가 => 각 적별로 발사체나 마법을 설정
public class EnemyAttack : MonoBehaviour
{
    [Title("참조")]
    [SerializeField] private EnemyFacade _facade;
    [SerializeField] private HitboxController _hitboxController;

    private readonly Dictionary<int, IActionStrategy> _strategies = new();
    private readonly List<int> _ids = new List<int>();
    private IActionStrategy _current;
    
    public int StrategyCount => _strategies.Count;
    public bool IsAttacking = false; //State의 판단기준

    public float LoopDelay => _current.LoopDelay;

    [Title("일단은 테스트")]
    [SerializeField] private EnemyWindup _arrow;
    [SerializeField] private EnemyWindup _magic;

    [SerializeField] private Transform _arrowSpawnPos;
    [SerializeField] private Transform _magicSpawnPos;
    public void Init()
    {
        IsAttacking = false;
        _facade = GetComponent<EnemyFacade>();
        //프로토타입의 전략 수립
        switch (_facade.Stat.EnemyType)
        {
            case EEnemyType.Warrior:
                {
                    RegisterStrategy(0, new ProtoMeleeAttack(_hitboxController));   //기본공격1
                    RegisterStrategy(1, new ProtoMeleeAttack(_hitboxController));   //기본공격2
                    break;
                }
            case EEnemyType.Archer:
                {
                    RegisterStrategy(3, new ProtoRangedAttack(_arrow, _arrowSpawnPos));   //화살공격
                    break;
                }
            case EEnemyType.Mage:
                {
                    RegisterStrategy(4, new ProtoRangedAttack(_magic,_magicSpawnPos));   //마법공격
                    break;
                }
            case EEnemyType.Boss:
                {
                    RegisterStrategy(2, new ProtoMeleeAttack(_hitboxController));   //콤보공격
                    break;
                }


        }
    }

    private void RegisterStrategy(int attackID, IActionStrategy strategy)
    {
        if (_strategies.ContainsKey(attackID))
        {
            Debug.LogWarning($"[EnemyAttack] AttackID {attackID} already registered.");
            return;
        }

        _strategies.Add(attackID, strategy);
        _ids.Add(attackID);
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

        OnAttackComplete();
    }

    public int GetRandomAttackID()
    {
        if (_ids.Count == 0)
        {
            return -1;
        }

        return _ids[Random.Range(0, _ids.Count)];
    }

    //공격 애니메이션 종료 => 다음 상태로 전환 가능
    public void OnAttackComplete()
    {
        _current = null;
        IsAttacking = false;
    }

    #region Animation Events
    //실질적인 공격은 애니메이션 이벤트를 통해 실행하며 각각 다른 방식으로 구현

    //근접 : 히트박스 활성화
    //궁수 : 공격준비
    //마법사 : 캐스팅
    public void OnBeginAttack()
    {
        _current?.BeginAction();
    }


    //근접 : 히트박스 비활성화 후 공격종료
    //궁수 : 발사체생성
    //마법사 : 발사체생성
    public void OnAttackEnd()
    {
        _current?.EndAction();
    }

    
    #endregion
}
