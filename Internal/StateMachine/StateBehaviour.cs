using ChroniclesExporter.StateMachine;

namespace ChroniclesExporter.Internal.StateMachine;

public abstract class StateBehaviour<E>(StateMachine<E> pStateMachine, E pId) : IState<E>
    where E : Enum
{
    /// <inheritdoc />
    public StateMachine<E> StateMachine { get; } = pStateMachine;
    /// <inheritdoc />
    public E Id { get; } = pId;
    /// <inheritdoc />
    public bool IsFirstFrame { get; protected set; }

    /// <inheritdoc/>
    public abstract void Update();
    /// <inheritdoc/>
    public virtual void Activate() { }
    /// <inheritdoc/>
    public virtual void Deactivate()
    {
        IsFirstFrame = true;
    }
}
