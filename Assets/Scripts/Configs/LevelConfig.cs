using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    #region Fields
    [SerializeField] float targetPoints;
    #endregion

    #region Getters
    public float TargetPoints => targetPoints;
    #endregion
}
