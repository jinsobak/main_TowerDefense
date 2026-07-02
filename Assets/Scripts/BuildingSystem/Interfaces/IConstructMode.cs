

public interface IConstructMode
{
    void OnEnter();
    void OnUpdate();
    void OnExit();

    void PerformMainAction();
    void CancelMainAction();
    void PerformSubAction();
}
