
public interface IPhaseTransition
{
    // 전환 조건 충족 여부
    bool ShouldTransition(EnemyController controller);
    //디버깅 : 전화조건을 설명
    string GetDescription();
}
