public abstract class State<T>
{
    abstract public void Begin(T e);
    abstract public void Execute(T e);
    abstract public void End(T e);
}
