using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigsManager : Singleton<ConfigsManager>
{
    #region Fields
    [SerializeField] GamePropertiesConfig gamePropertiesConfig;
    [SerializeField] ColorsConfig colorsConfig;
    [SerializeField] LevelsConfig levelsConfig;
    [SerializeField] BoostersConfig boostersConfig;
    [SerializeField] SoundsConfig soundsConfig;
    #endregion

    #region Getters
    public GamePropertiesConfig GamePropertiesConfig => gamePropertiesConfig;
    public ColorsConfig ColorsConfig => colorsConfig;
    public LevelsConfig LevelsConfig => levelsConfig;
    public BoostersConfig BoostersConfig => boostersConfig;
    public SoundsConfig SoundsConfig => soundsConfig;
    #endregion
}
