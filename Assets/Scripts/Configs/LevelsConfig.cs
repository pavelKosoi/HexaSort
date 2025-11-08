using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsConfig", menuName = "Configs/LevelsConfig")]
public class LevelsConfig : ScriptableObject
{
    #region Fields
    [SerializeField] LevelConfig[] levels;
    #endregion

    #region Getters
    public LevelConfig[] Levels => levels;
    #endregion
}
