namespace ChroniclesExporter.StateMachine;

public class State<E>(StateMachine<E> pStateMachine, E pId, Action pUpdate) : IState<E>
    where E : Enum
{
    protected StateMachine<E> StateMachine { get; } = pStateMachine;
    public E Id { get; } = pId;

    public bool IsFistFrame { get; private set; } = true;
    
    public void Update() => pUpdate.Invoke();
    public void Activate() { }
    public void Deactivate()
    {
        IsFistFrame = true;
    }
}
