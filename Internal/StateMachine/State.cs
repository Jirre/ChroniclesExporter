namespace ChroniclesExporter.StateMachine;

public class State<E>(StateMachine<E> pStateMachine, E pId, Action pUpdate) : IState<E>
    where E : Enum
{
    /// <summary>
    /// Parent State-Machine
    /// </summary>
    protected StateMachine<E> StateMachine { get; } = pStateMachine;
    /// <inheritdoc />
    public E Id { get; } = pId;
    
    /// <inheritdoc />
    public void Update() => pUpdate.Invoke();
    /// <inheritdoc />
    public void Activate() { }
    /// <inheritdoc />
    public void Deactivate() { }
}
