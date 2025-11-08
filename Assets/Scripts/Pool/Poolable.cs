using System.Collections;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public ObjectPool OriginPool {  get; set; }
    public Transform DefaultParent {  get; set; }
    public bool Inited { get; private set; }

    public virtual void Init() => Inited = true;
    public virtual void OnTakenFromPool() { }   
    public virtual void OnReturnedToPool() { }   

    public virtual void ReturnToPool()
    {
        OriginPool?.BackToPool(this);
    }
}
