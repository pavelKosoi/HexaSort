using UnityEngine;
using UnityEngine.EventSystems;

public class StackPickingState : BaseInputState
{
    public StackPickingState(InputManager inputManager) : base(inputManager) { }    
    public override void Enter()
    {
        Debug.Log("StackPicking mode activated");
    }

    public override void Exit()
    {
        Debug.Log("StackPicking mode exited");
    }

    public override void Tick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = inputManager.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<Stack>(out var stack))
                {
                    inputManager.OnStackPicked?.Invoke(stack);
                }
                else GameManager.Instance.BoosterManager.DeactivateBooster();
               
            }
            else GameManager.Instance.BoosterManager.DeactivateBooster();            
        }
    }
}
