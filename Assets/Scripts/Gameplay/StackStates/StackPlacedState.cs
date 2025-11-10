using DG.Tweening;
using UnityEngine;

public class StackPlacedState : BaseStackState
{
    public StackPlacedState(Stack stack) : base(stack) { }

    float moveSpeed = 5f;

    public override void Enter()
    {
        Vector3 startPos = stack.transform.position;
        Vector3 targetPos = stack.IdlePosition;

        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / moveSpeed;


        GameManager.Instance.StacksBar.RemoveStack(stack);
        stack.transform.DOMove(stack.PlacedPosition, duration).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            GameManager.Instance.FeedbackManager.Shake(stack.transform, 1.2f, 0.2f, stack.OnStackPlaced);
        });
    }
}