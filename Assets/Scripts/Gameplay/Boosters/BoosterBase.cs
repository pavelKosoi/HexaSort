using UnityEngine;

public abstract class BoosterBase
{
    protected BoosterConfig config;

    public BoosterBase(BoosterConfig config)
    {
        this.config = config;
    }

    public BoosterConfig Config => config;

    public abstract void OnActivate();
    public abstract void OnStackPicked(Stack stack);
    public virtual void OnCancel() { }
    public virtual void OnDeactivate() { }    
}
