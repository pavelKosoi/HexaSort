using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultInputState : BaseInputState
{
    IDragAndDropable currentMovable;

    public DefaultInputState(InputManager inputManager) : base(inputManager) { }    

    public override void Enter() { }
    public override void Exit()
    {
        currentMovable = null;
    }


    public void SetMovable(IDragAndDropable dragAndDropable)
    {
        currentMovable = dragAndDropable;
    }

    public override void Tick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = inputManager.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<IDragAndDropable>(out var movable))
                {
                    currentMovable = movable;
                    movable.OnPick();
                }
            }
        }
        else if (Input.GetMouseButton(0) && currentMovable != null)
        {
            if (Utilities.TryGetMouseWorldPosition(inputManager.MainCamera, out var targetPos))
                currentMovable.OnDrag(targetPos);
        }
        else if (Input.GetMouseButtonUp(0) && currentMovable != null)
        {
            currentMovable.OnDrop();
            currentMovable = null;
        }
    }
}
