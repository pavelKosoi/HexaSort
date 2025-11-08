using UnityEngine;
public class BaseHexCell : MonoBehaviour
{
   public Vector2Int coords;

    public void SetCoords(int q, int r)
    {
        coords = new Vector2Int(q, r);
    }


    [ContextMenu("GO")]
    void GetNeighbors()
    {
        GetComponentInParent<HexGrid>().GetNeighbors(this); 
    }
}
