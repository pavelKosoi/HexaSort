using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    private readonly Dictionary<Transform, Sequence> activeShakes = new();

    public void Shake(Transform target, float scaleAmount, float shakingTime)
    {
        Vector3 baseScale = target.localScale;

        if (activeShakes.TryGetValue(target, out Sequence oldSeq))
        {
            oldSeq.Kill(false);
            activeShakes.Remove(target);
        }

        Sequence seq = DOTween.Sequence();
        activeShakes[target] = seq;

        seq.Append(target.DOScale(baseScale * scaleAmount, shakingTime / 2f)
            .SetEase(Ease.OutQuad));

        seq.Append(target.DOScale(baseScale, shakingTime / 2f)
            .SetEase(Ease.InQuad));

        seq.OnComplete(() =>
        {
            target.localScale = baseScale;
            activeShakes.Remove(target);
        });

        seq.OnKill(() =>
        {
            activeShakes.Remove(target);
        });
    }
}
