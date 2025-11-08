using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    #region Fields
    [SerializeField] float targetPoints;
    [SerializeField] GameObject levelPrefab;
    #endregion

    #region Getters
    public float TargetPoints => targetPoints;
    public GameObject LevelPrefab => levelPrefab;
    #endregion
}
