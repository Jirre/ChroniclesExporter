namespace ChroniclesExporter.StateMachine;

public interface IState<E>
    where E : Enum
{
    /// <summary>
    /// Parent State Machine
    /// </summary>
    StateMachine<E> StateMachine { get; }
    
    /// <summary>
    /// Unique id of this state within the State Machine
    /// </summary>
    E Id { get; }

    /// <summary>
    /// Update Function called each update of the state machine if this state is active
    /// </summary>
    void Update();
    /// <summary>
    /// Function called upon activating the state through the state-machine
    /// </summary>
    void Activate();
    /// <summary>
    /// Function called when this state stops being the current state in the state machine
    /// </summary>
    void Deactivate();
}
