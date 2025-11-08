using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stack : MonoBehaviour, IDragAndDropable
{
    #region Fields
    [SerializeField] CapsuleCollider m_collider;

    List<HexView> hexes = new List<HexView>();
    private StateMachine stateMachine;
    #endregion

    #region Properties
    public Vector3 TargetPosition { get; private set; }
    public Vector3 IdlePosition { get; set; }

    #endregion

    #region Getters 
    public List<HexView> Hexes => hexes;
    #endregion

    #region UnityMethodes
    private void Awake()
    {
        stateMachine = new StateMachine();

        stateMachine.RegisterState(new StackDraggingState(this));
        stateMachine.RegisterState(new StackPlacedState(this));
        stateMachine.RegisterState(new StackIdleState(this));
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
    public void Build()
    {
        var gamePropertiesconfig = ConfigsManager.Instance.GamePropertiesConfig;
        int targetHeight = Random.Range(gamePropertiesconfig.MinStackHeight, gamePropertiesconfig.MaxStackHeight + 1);

        for (int i = 0; i < targetHeight; i++)
        {
            var newHex = ObjectPool.Instance.GetFromPool(ObjectPool.PoolObjectType.Hex, transform.position + Vector3.up *
                i * gamePropertiesconfig.DefaultHexThickness);
            newHex.transform.SetParent(transform);
            hexes.Add(newHex.GetComponent<HexView>());
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
    void AssignColorsToStack(List<HexView> hexes, Color[] colors)
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

    public void OnDrag(Vector3 position)
    {
        TargetPosition = position;
    }

    public void OnDrop()
    {
        HexGrid grid = GameManager.Instance.CurrentLevel.HexGrid;



        TargetPosition = Vector3.zero;
        stateMachine.ChangeState<StackIdleState>();
    }
    
    #endregion
}
