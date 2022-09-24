public class StateMachine<T>
{
    T Owner;
    State<T> currentState;
    State<T> previousState;
    
    public void Configure(T owner, State<T> _state)
    {
        Owner = owner;
        currentState = _state;
        currentState.Begin(owner);
    }

    public State<T> GetState()
    {
        return currentState;
    }

    public void Update()
    {
        if (currentState != null)
            currentState.Execute(Owner);
    }

    public void ChangeState(State<T> nextState)
    {
        previousState = currentState;
        if (currentState != null)
            currentState.End(Owner);
        currentState = nextState;
        if (currentState != null)
            currentState.Begin(Owner);
    }
     
    public void RevertState()
    {
        if (previousState != null)
            ChangeState(previousState);
    }

}
