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

        var possiblePairs = new List<(Stack from, Stack to)>();

        foreach (var neighbor in neighbors)
        {
            int fromGroups = stack.GetColorGroupCount();
            int toGroups = neighbor.GetColorGroupCount();

            bool bothSingleColor = fromGroups == 1 && toGroups == 1;

            if (bothSingleColor)
            {
                bool isStackBomb = stack.Cell is BombCell;
                bool isNeighborBomb = neighbor.Cell is BombCell;

                if (isStackBomb && !isNeighborBomb)
                {
                    possiblePairs.Add((from: neighbor, to: stack));
                    continue;
                }
                else if (!isStackBomb && isNeighborBomb)
                {
                    possiblePairs.Add((from: stack, to: neighbor));
                    continue;
                }
            }

            if (fromGroups == toGroups)
            {
                fromGroups = stack.Hexes.Count;
                toGroups = neighbor.Hexes.Count;
            }

            var pair = fromGroups > toGroups ? (from: stack, to: neighbor) : (from: neighbor, to: stack);
            possiblePairs.Add(pair);
        }

        if (possiblePairs.Count == 0) return null;


        var bestPair = possiblePairs.OrderBy(pair => pair.to.GetColorGroupCount())
            .ThenBy(pair => pair.to.Hexes.Count).First();

        return bestPair;
    }
}
