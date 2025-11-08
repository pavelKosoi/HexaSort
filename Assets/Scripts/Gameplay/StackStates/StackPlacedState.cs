using DG.Tweening;

public class StackPlacedState : BaseStackState
{
    public StackPlacedState(Stack stack) : base(stack) { }
    public override void Enter()
    {
        GameManager.Instance.StacksBar.RemoveStack(stack);
        stack.transform.DOMove(stack.PlacedPosition, 1f).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            stack.OnStackPlaced();
        });
    }
}