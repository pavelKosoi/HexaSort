using UnityEngine;

public interface IDragAndDropable
{
    void OnPick();
    void OnDrag(Vector3 position);
    void OnDrop();   
    
}
