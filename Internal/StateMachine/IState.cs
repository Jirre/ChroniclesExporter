namespace ChroniclesExporter.StateMachine;

public interface IState<E>
    where E : Enum
{
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
