using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexStackAnimator : MonoBehaviour
{
    public struct AnimationInfo
    {
        public Stack from;
        public Stack to;
        public Hex[] hexes;
        public Action onComplete;
    }

    #region Fields
    [SerializeField] float animationLength;
    [SerializeField] float arcHeight;
    #endregion

    #region Getters     
    float hexThickness => ConfigsManager.Instance.GamePropertiesConfig.DefaultHexThickness;
    #endregion

    #region HexMoning
    public void RunHexesMoving(AnimationInfo info) => StartCoroutine(MoveHexes(info));
    IEnumerator MoveHexes(AnimationInfo info)
    {
        var hexes = info.hexes.OrderByDescending(h => h.transform.position.y).ToList();
        var targetStack = info.to;

        var highestHex = targetStack.Hexes.Count > 0 ? targetStack.Hexes[targetStack.Hexes.Count - 1].transform : targetStack.transform;

        int count = hexes.Count;

        for (int i = 0; i < count; i++)
        {
            var hex = hexes[i];
            var startPos = hex.transform.position;
            var endPos = highestHex.position + Vector3.up * hexThickness * i;

            var dir = endPos - startPos;
            dir.y = 0;
            hex.transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);

            Vector3[] path = Utilities.BuildArc(startPos, endPos, arcHeight);
            hex.transform.position = path[0];

            bool isLast = (i == count - 1);
            hex.transform.SetParent(targetStack.transform);
            hex.transform.localScale = Vector3.one;

            GameManager.Instance.SoundsManager.PlaySoundOneShot(SoundType.Pop);

            var seq = DOTween.Sequence();
            seq.Append(hex.transform.DOPath(path, animationLength, PathType.CatmullRom).SetEase(Ease.Linear));
            seq.Join(hex.transform.DORotate(new Vector3(180, 0, 0), animationLength, RotateMode.LocalAxisAdd).SetEase(Ease.Linear))
               .OnComplete(() =>
               {
                   if (isLast)
                   {
                       info.from.Hexes.RemoveAll(h => info.hexes.Contains(h));
                       info.onComplete?.Invoke();
                   }
               });

            targetStack.Hexes.Add(hex);

            yield return new WaitForSeconds(animationLength / count);
        }
    }

    #endregion
}
