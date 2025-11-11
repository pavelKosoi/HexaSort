using UnityEngine;

public class HammerBooster : StackPickingUseBoosters
{
    public HammerBooster(BoosterConfig config) : base(config) { }
   
    public override void OnStackPicked(Stack stack)
    {
        if (stack.CurrentState is not StackPlacedState) return;

        stack.TryToPop(true);
        GameManager.Instance.InputManager.OnStackPicked -= OnStackPicked;
        GameManager.Instance.BoosterManager.DeactivateBooster();
        GameManager.Instance.BoosterManager.OnBoosterUsed(config.Type);
    }
}
