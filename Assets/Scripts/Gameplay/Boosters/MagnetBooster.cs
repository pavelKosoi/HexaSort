using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagnetBooster : StackPickingUseBoosters
{
    public MagnetBooster(BoosterConfig config) : base(config) { }    

    public override void OnStackPicked(Stack stack)
    {
        if (stack.CurrentState is not StackPlacedState) return;

        var allStacks = GameManager.Instance.CurrentLevel.HexGrid.AllCells
            .Where(c => c.IsOccupied && c.OccupiedBy != stack).Select(c => c.OccupiedBy).ToList();


        if (allStacks.Count == 0)
        {
            GameManager.Instance.BoosterManager.DeactivateBooster();
            return;
        }

        var targetColor = stack.UpperColor;

        var bestStack = allStacks.OrderByDescending(s => s.GetSameTopColorCount(targetColor))
            .ThenBy(s => s.GetColorGroupCount()).FirstOrDefault();


        if (bestStack == null || bestStack.GetSameTopColorCount(targetColor) == 0)
        {
            GameManager.Instance.BoosterManager.DeactivateBooster();
            return;
        }

        var matchingHexes = bestStack.Hexes.Where(h => h.Color == targetColor).Reverse()
            .Take(bestStack.GetSameTopColorCount(targetColor)).Reverse().ToArray();


        if (matchingHexes.Length == 0)
        {
            GameManager.Instance.BoosterManager.DeactivateBooster();
            return;
        }

        GameManager.Instance.BoosterManager.OnBoosterUsed(config.Type);
        GameManager.Instance.BoosterManager.DeactivateBooster();

        GameManager.Instance.HexStackAnimator.RunHexesMoving(new HexStackAnimator.AnimationInfo
        {
            from = bestStack,
            to = stack,
            hexes = matchingHexes,
            onComplete = () =>
            {
                bestStack.TryToDestroy();
                stack.TryToPop();
                GameManager.Instance.CurrentLevel.CheckAllMatches();               
            }
        });
    }
}
