using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchSelector
{
    private readonly HexGrid hexGrid;

    public MatchSelector(HexGrid grid)
    {
        hexGrid = grid;
    }

    public (Stack from, Stack to)? FindBestMatchPair(BaseHexCell cell)
    {
        var stack = cell.OccupiedBy;
        if (stack == null) return null;

        var neighbors = hexGrid.GetNeighbors(cell).Where(n => n.IsOccupied)
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
            .ThenBy(pair => pair.to.Hexes.Count).FirstOrDefault();

        return bestPair;
    }
}
