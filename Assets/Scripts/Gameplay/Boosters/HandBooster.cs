using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBooster : BoosterBase
{
    public HandBooster(BoosterConfig config) : base(config) { }    

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

        GameManager.Instance.InputManager.SetInputMode<DefaultInputState>();

        var inputState = GameManager.Instance.InputManager.StateMachine.GetState<DefaultInputState>();
        (inputState as DefaultInputState).SetMovable(stack);
        stack.ForceToPick();
        GameManager.Instance.InputManager.OnStackPicked -= OnStackPicked;
        GameManager.Instance.BoosterManager.DeactivateBooster();
        GameManager.Instance.BoosterManager.OnBoosterUsed(config.Type);
    }    
}
