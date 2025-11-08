public abstract class BaseGameState : IState
{
    protected GameManager gameManager;

    protected BaseGameState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Tick() { }
}
