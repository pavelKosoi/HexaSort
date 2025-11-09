using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackPopingState : BaseStackState
{
    public StackPopingState(Stack stack) : base(stack) { }

    public override void Enter()
    {
        stack.StartCoroutine(PopStack());
    }

    IEnumerator PopStack()
    {
        stack.Cell.OnStackPopped();
        stack.Cell.Vacate();

        var sortedHexes = stack.Hexes.OrderByDescending(h => h.transform.position.y).ToList();
        var tweens = new List<Tween>();
        var popVFX = ObjectPool.Instance.GetFromPool(ObjectPool.PoolObjectType.HexPopFx, stack.transform.position) as PoolableVFX;

        foreach (var item in sortedHexes)
        {
            GameManager.Instance.SoundsManager.PlaySoundOneShot(SoundType.Pop1);
            popVFX.transform.position = item.transform.position;
            popVFX.Play();

            var tween = item.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuint)
           .OnComplete(() =>
           {

               item.ReturnToPool();
           });

            tweens.Add(tween);
            GameManager.Instance.CurrentLevel.AddPoints(1);

            yield return new WaitForSeconds(0.5f / sortedHexes.Count);
        }

        yield return new WaitUntil(() => tweens.TrueForAll(t => !t.IsActive()));
        popVFX.ReturnToPool();
        GameObject.Destroy(stack.gameObject);
    }

}
