using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackIdleState : BaseStackState
{
    public StackIdleState(Stack stack) : base(stack) { }
    Tween tween;
    bool firstTime = true;
    float moveSpeed = 8f;

    public override void Enter()
    {
        Vector3 startPos = stack.transform.position;
        Vector3 targetPos = stack.IdlePosition;

        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / moveSpeed;

        tween = stack.transform.DOMove(targetPos, duration)
            .SetEase(firstTime ? Ease.InQuad : Ease.Linear).OnComplete(() =>
        {
            firstTime = false;
            GameManager.Instance.FeedbackManager.Shake(stack.transform, 1.2f, 0.2f);
        });


    }

    public override void Exit()
    {
        tween.Kill();
    }
}
