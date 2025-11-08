using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    #region Fields
    BaseHexCell[] cells;
    Dictionary<Vector2Int, BaseHexCell> cellMap = new();
    List<BaseHexCell> lastNeighbors = new List<BaseHexCell>();
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
            Vector2Int axial = WorldToHexFlatTop(transform.TransformPoint(cell.transform.position), 0.5f);
            cellMap[axial] = cell;
            cell.SetCoords(axial.x, axial.y);
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

    public List<BaseHexCell> GetNeighbors(BaseHexCell cell)
    {
        var dirs = new Vector2Int[]
        {
            new(1, 0), new(1, -1), new(0, -1),
            new(-1, 0), new(-1, 1), new(0, 1)
        };

        List<BaseHexCell> neighbors = new();
        foreach (var dir in dirs)
        {
            Vector2Int neighborPos = new Vector2Int(cell.coords.x + dir.x, cell.coords.y + dir.y);
            if (cellMap.TryGetValue(neighborPos, out var neighbor))
                neighbors.Add(neighbor);
        }
        lastNeighbors = neighbors;
        return neighbors;
    }  
}
