using ChroniclesExporter.StateMachine;

namespace ChroniclesExporter.Internal.StateMachine;

public abstract class StateBehaviour<E>(StateMachine<E> pStateMachine, E pId) : IState<E>
    where E : Enum
{
    /// <summary>
    /// Parent State-Machine
    /// </summary>
    protected StateMachine<E> StateMachine { get; } = pStateMachine;
    /// <inheritdoc />
    public E Id { get; } = pId;

    /// <inheritdoc/>
    public abstract void Update();
    /// <inheritdoc/>
    public virtual void Activate() { }
    /// <inheritdoc/>
    public virtual void Deactivate() { }
}
