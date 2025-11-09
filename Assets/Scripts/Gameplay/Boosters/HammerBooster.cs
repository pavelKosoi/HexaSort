using UnityEngine;

public class HammerBooster : BoosterBase
{
    public HammerBooster(BoosterConfig config) : base(config) { }

    public override void OnActivate()
    {
        GameManager.Instance.InputManager.OnStackPicked += OnStackPicked;
    }
    public override void OnCancel()
    {
        GameManager.Instance.InputManager.OnStackPicked -= OnStackPicked;
    }
    public override void OnStackPicked(Stack stack)
    {
        if (stack.CurrentState is not StackPlacedState) return;

        stack.TryToPop(true);
        GameManager.Instance.InputManager.OnStackPicked -= OnStackPicked;
        GameManager.Instance.BoosterManager.DeactivateBooster();
        GameManager.Instance.BoosterManager.OnBoosterUsed(config.Type);
    }
}
