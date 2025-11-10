using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Stack : MonoBehaviour, IDragAndDropable
{
    #region Fields
    [SerializeField] CapsuleCollider m_collider;

    List<Hex> hexes = new List<Hex>();
    StateMachine stateMachine;
    BaseHexCell lastSelectedCell;
    BaseHexCell cell;

    #endregion

    #region Properties
    public Vector3 TargetPosition { get; private set; }
    public Vector3 IdlePosition { get; private set; }
    public Vector3 PlacedPosition { get; private set; }

    #endregion

    #region Getters 
    public List<Hex> Hexes => hexes;
    public Color UpperColor => hexes[hexes.Count - 1].Color;
    public IState CurrentState => stateMachine.CurrentState;
    public BaseHexCell Cell => cell;
    HexGrid grid => GameManager.Instance.CurrentLevel.HexGrid;
    #endregion

    #region UnityMethodes
    private void Awake()
    {
        stateMachine = new StateMachine();

        stateMachine.RegisterState(new StackDraggingState(this));
        stateMachine.RegisterState(new StackPlacedState(this));
        stateMachine.RegisterState(new StackIdleState(this));
        stateMachine.RegisterState(new StackPopingState(this));
    }

    private void Start()
    {
        stateMachine.ChangeState<StackIdleState>();
    }

    private void Update()
    {
        stateMachine.Update();
    } 

    #endregion

    #region Build
    public void SetIdlePosition(Vector3 position) => IdlePosition = position;

    public void Build()
    {
        var gamePropertiesconfig = ConfigsManager.Instance.GamePropertiesConfig;
        int targetHeight = Random.Range(gamePropertiesconfig.MinStackHeight, gamePropertiesconfig.MaxStackHeight + 1);

        for (int i = 0; i < targetHeight; i++)
        {
            var newHex = ObjectPool.Instance.GetFromPool(ObjectPool.PoolObjectType.Hex, transform.position + Vector3.up *
                i * gamePropertiesconfig.DefaultHexThickness);
            newHex.transform.SetParent(transform);
            hexes.Add(newHex.GetComponent<Hex>());
        }

        Vector3 start = hexes[0].transform.position;
        Vector3 end = hexes[hexes.Count - 1].transform.position;
        Vector3 worldCenter = (start + end) / 2f;

        m_collider.center = m_collider.transform.InverseTransformPoint(worldCenter);
        float height = Vector3.Distance(start, end);
        m_collider.height = height;

        int colorsCount = Random.Range(gamePropertiesconfig.MinColorsInStack, gamePropertiesconfig.MaxColorsInStack + 1);
        Color[] colors = ConfigsManager.Instance.ColorsConfig.GetRandomColors(colorsCount);
        AssignColorsToStack(hexes, colors);

    }
    void AssignColorsToStack(List<Hex> hexes, Color[] colors)
    {
        int total = hexes.Count;
       
        int[] distribution = new int[colors.Length];
        int remaining = total;

        for (int i = 0; i < colors.Length; i++)
        {
            if (i == colors.Length - 1)
            {
                distribution[i] = remaining;
            }
            else
            {
                int maxForThis = Mathf.Max(1, remaining - (colors.Length - i - 1)); 
                int count = Random.Range(1, maxForThis + 1);
                distribution[i] = count;
                remaining -= count;
            }
        }

        System.Random rnd = new System.Random();
        colors = colors.OrderBy(c => rnd.Next()).ToArray();

        int hexIndex = 0;
     
        for (int i = 0; i < colors.Length; i++)
        {
            for (int j = 0; j < distribution[i]; j++)
            {
               
                if (hexIndex >= hexes.Count) return;

                hexes[hexIndex].SetColor(colors[i]);
                //hexes[hexIndex].SetColor(Color.blue);

                hexIndex++;
            }
        }
    }
    #endregion

    #region Moving
    public void OnPick()
    {
        if (stateMachine.CurrentState is StackPlacedState) return;
        stateMachine.ChangeState<StackDraggingState>();
    }
    public void ForceToPick()
    {
        cell.Vacate();
        stateMachine.ChangeState<StackDraggingState>();
    }

    public void OnDrag(Vector3 position)
    {
        TargetPosition = position + Vector3.forward * 0.5f;
        var nearestCell = grid.GetNearestCell(TargetPosition);
        if (nearestCell.Item1 != null && nearestCell.Item2)
        {
            if (!nearestCell.Item1.IsOccupied && nearestCell.Item1 != lastSelectedCell)
            {
                if(lastSelectedCell != null) lastSelectedCell.SetDefaultColor();
                nearestCell.Item1.SetSelectedColor();
                lastSelectedCell = nearestCell.Item1;
            }
        }
        else if (lastSelectedCell != null)
        {
            lastSelectedCell.SetDefaultColor();
            lastSelectedCell= null;
        }
    }

    public void OnDrop()
    {
        var result = grid.GetNearestCell(TargetPosition);
        var nearestCell = result.Item1;
        bool isIinsdeOfCell = result.Item2;

        if (cell != null)
        {
            nearestCell = cell;
            isIinsdeOfCell = true;
        }
        if (nearestCell != null)
        {
            if (isIinsdeOfCell && !nearestCell.IsOccupied)
            {               
                PlacedPosition = nearestCell.transform.position + Vector3.up * ConfigsManager.Instance.GamePropertiesConfig.DefaultHexThickness;
                IdlePosition = PlacedPosition;
                cell = nearestCell;
                nearestCell.Occupy(this);
                nearestCell.SetDefaultColor();
                stateMachine.ChangeState<StackPlacedState>();
            }
            else stateMachine.ChangeState<StackIdleState>();
        }        
        else stateMachine.ChangeState<StackIdleState>();

        TargetPosition = Vector3.zero;
        lastSelectedCell = null;
    }    
    
    public void OnStackPlaced()
    {
        transform.SetParent(cell.transform);
        GameManager.Instance.CurrentLevel.CheckAllMatches();
    }

    #endregion

    #region Management

    public int GetColorGroupCount()
    {
        int groups = 1;
        for (int i = 1; i < Hexes.Count; i++)
        {
            if (Hexes[i].Color != Hexes[i - 1].Color) groups++;
        }
        return groups;
    }

    public void TryToDestroy()
    {
        if (hexes.Count == 0)
        {
            cell.Vacate();
            Destroy(gameObject);
        }       
    }


    public void TryToPop(bool forceToPop = false)
    {
        if (forceToPop || (hexes.Count >= ConfigsManager.Instance.GamePropertiesConfig.StackTargetHeight
            && hexes.All(h => h.Color == UpperColor)))
        {
            stateMachine.ChangeState<StackPopingState>();            
        }
    }
  
    #endregion
}
