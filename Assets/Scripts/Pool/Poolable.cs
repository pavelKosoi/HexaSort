using System.Collections;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    [SerializeField] bool auomaticReturn;
    [SerializeField] protected float returnDelay;

    public ObjectPool OriginPool {  get; set; }
    public Transform DefaultParent {  get; set; }
    public bool Inited { get; private set; }

    public virtual void Init() => Inited = true;
   
    public virtual void ReturnToPool()
    {
        OriginPool?.BackToPool(this);
    }  
    public virtual void OnTakenFromPool()
    {
        if (auomaticReturn) StartCoroutine(DelayReturnRoutine(returnDelay));
    }
    public virtual void OnReturnedToPool() { }

    public void ReturnToPool(float delay)
    {
        StartCoroutine(DelayReturnRoutine(delay));
    }

    IEnumerator DelayReturnRoutine(float delay)
    {
        yield return new WaitForSeconds(returnDelay);
        OnAutomaticReturnTimersUp();
    }
    protected virtual void OnAutomaticReturnTimersUp()
    {
        ReturnToPool();
    }

}
