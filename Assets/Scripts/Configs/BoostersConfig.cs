using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoostersConfig", menuName = "Configs/BoostersConfig")]
public class BoostersConfig : ScriptableObject
{
    #region Fields
    [SerializeField] BoosterConfig[] boosters;
    #endregion

    #region Getters
    public BoosterConfig[] Boosters => boosters;   
    #endregion
}
