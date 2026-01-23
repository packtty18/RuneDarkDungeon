public interface IAttackStretagy
{
    float LoopDelay { get; }
    void AttackReady();
    void AttackExecute();
}
