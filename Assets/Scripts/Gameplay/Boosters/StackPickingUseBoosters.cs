using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StackPickingUseBoosters : BoosterBase
{
    protected StackPickingUseBoosters(BoosterConfig config) : base(config) { }    

    public override void OnActivate() => GameManager.Instance.InputManager.OnStackPicked += OnStackPicked;
    public override void OnCancel() => GameManager.Instance.InputManager.OnStackPicked -= OnStackPicked;    
}
