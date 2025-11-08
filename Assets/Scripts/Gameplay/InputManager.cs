using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    private IDragAndDropable currentMovable;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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
            if (Utilities.TryGetMouseWorldPosition(mainCamera, out var targetPos))
            {
                currentMovable.OnDrag(targetPos);
            }
        }

        else if (Input.GetMouseButtonUp(0) && currentMovable != null)
        {

            currentMovable.OnDrop();
            currentMovable = null;
        }
    }
}
