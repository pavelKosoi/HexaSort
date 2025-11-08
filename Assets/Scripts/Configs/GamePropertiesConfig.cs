using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePropertiesConfig", menuName = "Configs/GamePropertiesConfig")]
public class GamePropertiesConfig : ScriptableObject
{
    #region Fields
    [SerializeField] float defaultHexThickness;
    [SerializeField] int maxStackHeight;
    [SerializeField] int minStackHeight;
    [SerializeField] int maxColorsInStack;
    [SerializeField] int minColorsInStack;
    [SerializeField] GameObject hexPrefab;
    #endregion

    #region Getters
    public float DefaultHexThickness => defaultHexThickness;
    public int MaxStackHeight => maxStackHeight;
    public int MinStackHeight => minStackHeight;
    public GameObject HexPrefab => hexPrefab;
    public int MinColorsInStack => minColorsInStack;
    public int MaxColorsInStack => maxColorsInStack;
    #endregion
}
