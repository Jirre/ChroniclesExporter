namespace ChroniclesExporter.StateMachine;

public class StateMachine<E>
    where E : Enum
{
    private readonly Dictionary<E, IState<E>> _states =  new Dictionary<E, IState<E>>();
    private readonly List<E> _stateOrder = new List<E>();

    private IState<E>? _currentState;

    /// <summary>
    /// Call the update function of the currently active state (if any)
    /// </summary>
    public void Update()
    {
        if (_currentState == null)
            return;
        
        _currentState.Update();
    }

    /// <summary>
    /// Checks if a state is currently active within the state-machine
    /// </summary>
    public bool IsRunning() => _currentState != null;

    #region --- Access ---

    /// <summary>
    /// Is the current state the same as the given one
    /// </summary>
    /// <param name="pId">The state to check against the current state</param>
    public bool IsCurrentState(E pId) =>
        _currentState?.Id.Equals(pId) ?? false;
    
    /// <summary>
    /// Does the state machine contain a state with the given name
    /// </summary>
    /// <param name="pId">The Id of the state to check for</param>
    public bool HasState(E pId) => _states.ContainsKey(pId);

    /// <summary>
    /// Returns the state within the state list if it exists
    /// </summary>
    /// <param name="pId">ID of the state to return</param>
    /// <param name="pState">Output of the state if found</param>
    /// <returns>Whether the state exists or not</returns>
    public bool TryGetState(E pId, out IState<E> pState) =>
        _states.TryGetValue(pId, out pState!);

    #endregion
    
    #region --- Registration ---

    /// <summary>
    /// Adds state to state-machine
    /// </summary>
    public void Add(IState<E> pState)
    {
        if (_states.ContainsKey(pState.Id))
            throw new ArgumentException($"State of key [{pState.Id}] already added to statemachine");
        _states.Add(pState.Id, pState);
        _stateOrder.Add(pState.Id);
    }
    /// <summary>
    /// Adds a new state with the provided Id and Update Function as a new state to the state-machine
    /// </summary>
    public void Add(E pId, Action pFunc) => Add(new State<E>(this, pId, pFunc));

    #endregion

    #region --- Navigation ---

    /// <summary>
    /// Sets the state of this state machine to the given state
    /// </summary>
    /// <param name="pState">State Object to set the state machine to</param>
    public void Goto(IState<E> pState)
    {
        if (_currentState == pState ||
            !_states.ContainsKey(pState.Id))
            return;

        _currentState?.Deactivate();
        _currentState = pState;
        _currentState.Activate();
    }
    
    /// <summary>
    /// Sets the state of this state machine to the state with the given id
    /// </summary>
    /// <param name="pId">ID of the state to set the state machine to</param>
    public void Goto(E pId)
    {
        if (!_states.TryGetValue(pId, out IState<E>? state))
            throw new ArgumentOutOfRangeException($"No State with Id [{pId}] was found in the statemachine");

        Goto(state);
    }

    #endregion
}
