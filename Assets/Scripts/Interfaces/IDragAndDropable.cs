using UnityEngine;

public interface IDragAndDropable
{
    bool OnPick();
    void OnDrag(Vector3 position);
    void OnDrop();   
    
}
