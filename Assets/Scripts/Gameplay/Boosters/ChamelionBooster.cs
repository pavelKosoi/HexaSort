using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChamelionBooster : StackPickingUseBoosters
{
    public ChamelionBooster(BoosterConfig config) : base(config) { }        

    public override void OnStackPicked(Stack stack)
    {
        var cell = stack.Cell;

        var neighbors = GameManager.Instance.CurrentLevel.HexGrid
            .GetNeighbors(cell).Where(n => n.IsOccupied).Select(n => n.OccupiedBy).ToList();

        if (neighbors.Count == 0)
        {
            GameManager.Instance.BoosterManager.DeactivateBooster();
            return;
        }

        var bestNeighbor = neighbors.OrderByDescending(s => s.GetSameTopColorCount(s.UpperColor))
            .ThenBy(s => s.GetColorGroupCount()).FirstOrDefault();


        if (bestNeighbor == null)
        {
            GameManager.Instance.BoosterManager.DeactivateBooster();
            return;
        }

        var targetColor = bestNeighbor.UpperColor;

        stack.StartCoroutine(ChangeColorStack(stack, targetColor));
    }

    IEnumerator ChangeColorStack(Stack stack, Color targetColor)
    {
        GameManager.Instance.BoosterManager.OnBoosterUsed(config.Type);
        GameManager.Instance.BoosterManager.DeactivateBooster();

        for (int i = stack.Hexes.Count - 1; i >= 0; i--)
        {
            var hex = stack.Hexes[i];
            hex.SetColor(targetColor);
            GameManager.Instance.FeedbackManager.Shake(hex.transform, 1.2f, 0.1f);
            yield return new WaitForSeconds(1f / stack.Hexes.Count);
        }        

        GameManager.Instance.CurrentLevel.CheckAllMatches();
       
    }
}
