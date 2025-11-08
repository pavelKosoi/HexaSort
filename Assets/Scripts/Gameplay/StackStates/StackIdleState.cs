using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackIdleState : BaseStackState
{
    public StackIdleState(Stack stack) : base(stack) { }

    Tween tween;

    public override void Enter()
    {
        tween = stack.transform.DOMove(stack.IdlePosition, 1f).SetEase(Ease.OutQuint);
    }

    public override void Exit()
    {
        tween.Kill();
    }
}
