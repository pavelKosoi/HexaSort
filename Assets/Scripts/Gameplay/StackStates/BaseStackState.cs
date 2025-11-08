public abstract class BaseStackState : IState
{
    protected Stack stack;

    protected BaseStackState(Stack stack)
    {
        this.stack = stack;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Tick() { }
}
