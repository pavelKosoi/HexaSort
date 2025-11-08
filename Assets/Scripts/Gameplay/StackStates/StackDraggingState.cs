using UnityEngine;

public class StackDraggingState : BaseStackState
{
    public StackDraggingState(Stack stack) : base(stack) { }

    public override void Enter()
    {
        foreach (var item in stack.Hexes)
        {
            item.HexView.gameObject.layer = LayerMask.NameToLayer("PointedStack");
        }
    }
    public override void Exit()
    {

        foreach (var item in stack.Hexes)
        {
            item.HexView.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public override void Tick()
    {
        if (stack.TargetPosition.Equals(Vector3.zero)) return;
        Vector3 pos = Vector3.Lerp(stack.transform.position, stack.TargetPosition, Time.deltaTime * 50f);
        stack.transform.position = pos;
    }
}