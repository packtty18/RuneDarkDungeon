public interface IActionStrategy
{
    float LoopDelay { get; }
    void BeginAction();
    void EndAction();
}
