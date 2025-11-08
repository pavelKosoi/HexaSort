using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    MatchSelector matchSelector;
    Coroutine matchRoutine;

    public Action OnLevelComplete;
    public Action OnLevelFailed;

    public HexGrid HexGrid {  get; private set; }


    private void Awake()
    {
        HexGrid = GetComponentInChildren<HexGrid>();
        matchSelector = new MatchSelector(HexGrid);

    }

    public void CheckAllMatches()
    {
        if (matchRoutine != null) return;
        matchRoutine = StartCoroutine(CheckAllMatchesCoroutine());
    }

    private IEnumerator CheckAllMatchesCoroutine()
    {
        bool hasMatches;

        do
        {
            hasMatches = false;

            foreach (var cell in HexGrid.AllCells)
            {
                if (!cell.IsOccupied) continue;

                var match = matchSelector.FindBestMatchPair(cell);
                if (match == null) continue;

                hasMatches = true;

                var (from, to) = match.Value;

                Hex[] stackHexes = from.Hexes.Reverse<Hex>()
                    .TakeWhile(h => h.Color == from.UpperColor).Reverse().ToArray();

                bool done = false;

                GameManager.Instance.HexStackAnimator.RunHexesMoving(new HexStackAnimator.AnimationInfo
                {
                    from = from,
                    to = to,
                    hexes = stackHexes,
                    onComplete = () =>
                    {
                        from.TryToDestroy();
                        to.TryToPop();
                        done = true;
                    }
                });

                yield return new WaitUntil(() => done);
            }

        } while (hasMatches);

        matchRoutine = null;
    }



    public (Stack from, Stack to)? FindBestMatchPair(BaseHexCell cell)
    {
        var stack = cell.OccupiedBy;
        if (stack == null) return null;

        var neighbors = HexGrid.GetNeighbors(cell).Where(n => n.IsOccupied)
            .Select(n => n.OccupiedBy).Where(s => s.UpperColor == stack.UpperColor).ToList();

        if (neighbors.Count == 0) return null;

        var bestPair = neighbors
            .Select(neighbor =>
            {
                int fromGroups = stack.GetColorGroupCount();
                int toGroups = neighbor.GetColorGroupCount();

                if (fromGroups == toGroups)
                {
                    fromGroups = stack.Hexes.Count;
                    toGroups = neighbor.Hexes.Count;
                }

                return fromGroups > toGroups ? (from: stack, to: neighbor) : (from: neighbor, to: stack);

            })
            .OrderBy(pair => pair.to.GetColorGroupCount())
            .FirstOrDefault();

        return bestPair;
    }

}
