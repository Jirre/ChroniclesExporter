using ChroniclesExporter.StateMachine;

namespace ChroniclesExporter.Internal.StateMachine;

public abstract class StateBehaviour<E>(StateMachine<E> pStateMachine, E pId) : IState<E>
    where E : Enum
{
    protected StateMachine<E> StateMachine { get; } = pStateMachine;
    public E Id { get; } = pId;

    public bool IsFistFrame { get; protected set; }

    /// <inheritdoc/>
    public abstract void Update();
    /// <inheritdoc/>
    public virtual void Activate() { }
    /// <inheritdoc/>
    public virtual void Deactivate()
    {
        IsFistFrame = true;
    }
}
