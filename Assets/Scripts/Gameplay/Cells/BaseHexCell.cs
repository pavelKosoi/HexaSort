using UnityEngine;
public class BaseHexCell : MonoBehaviour
{
    #region Fields
    Vector2Int coords;
    HexView hexView;
    #endregion

    #region Properties    
    public Stack OccupiedBy { get; private set; }

    #endregion

    #region Getters   
    public Vector2Int Coordinates => coords;
    public virtual bool IsOccupied => OccupiedBy != null;
    #endregion


    #region UnityMethodes
    private void Awake()
    {
        hexView = GetComponentInChildren<HexView>();
    }
    #endregion


    public void SetCoords(Vector2Int coordinates)
    {
        coords = coordinates;
    }

    public void Occupy(Stack stack)
    {        
        OccupiedBy = stack;
    }

    public void Vacate()
    {     
        OccupiedBy = null;
    }

    public virtual void SetDefaultColor() => SetColor(ConfigsManager.Instance.ColorsConfig.DefaultCellColor);
    public virtual void SetSelectedColor() => SetColor(ConfigsManager.Instance.ColorsConfig.SelectedCellColor);

    protected virtual void SetColor(Color color) => hexView.SetColor(color);
}
