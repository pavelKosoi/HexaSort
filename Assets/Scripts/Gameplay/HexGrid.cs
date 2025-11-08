using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    #region Fields
    BaseHexCell[] cells;
    Dictionary<Vector2Int, BaseHexCell> cellMap = new();
    List<BaseHexCell> lastNeighbors = new List<BaseHexCell>();
    readonly List<BaseHexCell> tempNeighbors = new(6);
    readonly Vector2Int[] neighborDirs = new Vector2Int[]
    {
        new(1, 0), new(1, -1), new(0, -1),
        new(-1, 0), new(-1, 1), new(0, 1)
    };
    #endregion

    #region Getters
    float hexRadius => ConfigsManager.Instance.GamePropertiesConfig.DefaultHexRadius;
    public BaseHexCell[] AllCells => cells;
    public bool AllCellsFilled => cells.All(c => c.IsOccupied);
    #endregion

    #region UnityMethodes
    void Awake()
    {
        cells = GetComponentsInChildren<BaseHexCell>();
        InitialCellsCoords();
       
    }

    void OnDrawGizmos()
    {
        if (cells == null)
            return;

        Gizmos.color = Color.yellow;

        foreach (var cell in lastNeighbors)
        {
            Gizmos.DrawSphere(cell.transform.position + Vector3.up * 0.05f, 0.05f);

        }
    }
    #endregion

    #region Mapping
    void InitialCellsCoords()
    {
        foreach (var cell in cells)
        {
            Vector2Int axial = WorldToHexFlatTop(transform.TransformPoint(cell.transform.position), hexRadius);               

            cellMap[axial] = cell;
            cell.SetCoords(axial);
            cell.name = $"Hex ({axial.x},{axial.y})";
        }
    }

    Vector2Int WorldToHexFlatTop(Vector3 position, float radius)
    {
        float q = (2f / 3f * position.x) / radius;
        float r = (-1f / 3f * position.x + Mathf.Sqrt(3f) / 3f * position.z) / radius;
        return CubeRound(q, r);
    }

    Vector2Int CubeRound(float q, float r)
    {
        float x = q;
        float z = r;
        float y = -x - z;

        int rx = Mathf.RoundToInt(x);
        int ry = Mathf.RoundToInt(y);
        int rz = Mathf.RoundToInt(z);

        float x_diff = Mathf.Abs(rx - x);
        float y_diff = Mathf.Abs(ry - y);
        float z_diff = Mathf.Abs(rz - z);

        if (x_diff > y_diff && x_diff > z_diff)
        {
            rx = -ry - rz;
        }
        else if (y_diff > z_diff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Vector2Int(rx, rz);
    }
    #endregion


    #region CellsManagement
    public List<BaseHexCell> GetNeighbors(BaseHexCell cell, bool drawNeighbors = false)
    {
        var dirs = new Vector2Int[]
        {
            new(1, 0), new(1, -1), new(0, -1),
            new(-1, 0), new(-1, 1), new(0, 1)
        };

        List<BaseHexCell> neighbors = new();
        foreach (var dir in dirs)
        {
            Vector2Int neighborPos = new Vector2Int(cell.Coordinates.x + dir.x, cell.Coordinates.y + dir.y);
            if (cellMap.TryGetValue(neighborPos, out var neighbor))
                neighbors.Add(neighbor);
        }
       if(drawNeighbors) lastNeighbors = neighbors;
        return neighbors;
    }
    public void GetNeighbors(Vector2Int coords, List<BaseHexCell> buffer)
    {
        for (int i = 0; i < neighborDirs.Length; i++)
        {
            Vector2Int neighborPos = coords + neighborDirs[i];
            if (cellMap.TryGetValue(neighborPos, out var neighbor))
                buffer.Add(neighbor);
        }
    }


    public (BaseHexCell, bool) GetNearestCell(Vector3 position)
    {
        Vector2Int axial = WorldToHexFlatTop(position, 0.5f);

        if (cellMap.TryGetValue(axial, out var exactCell))
        {
            bool inside = IsPointInsideHex(position, exactCell.transform.position, 0.5f);
            return (exactCell, inside);
        }

        tempNeighbors.Clear();
        GetNeighbors(axial, tempNeighbors);

        float bestDist = float.MaxValue;
        BaseHexCell nearest = null;

        for (int i = 0; i < tempNeighbors.Count; i++)
        {
            var cell = tempNeighbors[i];
            float dist = (cell.transform.position - position).sqrMagnitude;
            if (dist < bestDist)
            {
                bestDist = dist;
                nearest = cell;
            }
        }

        if (nearest == null)
            return (null, false);

        bool insideNearest = IsPointInsideHex(position, nearest.transform.position, 0.5f);
        return (nearest, insideNearest);
    }
   

    bool IsPointInsideHex(Vector3 point, Vector3 center, float radius)
    {
        Vector3 local = point - center;
        float q2x = Mathf.Abs(local.x) * 0.57735f; // sqrt(3)/3
        float q2y = Mathf.Abs(local.z);

        return (q2x + q2y <= radius);
    }
    #endregion

}
