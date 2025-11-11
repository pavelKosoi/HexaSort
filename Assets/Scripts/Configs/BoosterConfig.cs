using UnityEngine;

[CreateAssetMenu(fileName = "BoosterConfig", menuName = "Configs/BoosterConfig")]
public class BoosterConfig : ScriptableObject
{
    #region Fields
    [SerializeField] string boosterName;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] BoosterType type;
    [SerializeField] Vector2 iconSize;

    #endregion

    #region Getters
    public string BoosterName => boosterName;
    public string Description => description;
    public Sprite Icon => icon;
    public BoosterType Type => type;
    public Vector2 IconSize => iconSize;
    #endregion
}

public enum BoosterType
{
    Hammer,
    Hand,
    Chamelion,
    Magnet
}
