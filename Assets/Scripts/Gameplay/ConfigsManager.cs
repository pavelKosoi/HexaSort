using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigsManager : Singleton<ConfigsManager>
{
    #region Fields
    [SerializeField] GamePropertiesConfig gamePropertiesConfig;
    [SerializeField] ColorsConfig colorsConfig;
    #endregion

    #region Getters
    public GamePropertiesConfig GamePropertiesConfig => gamePropertiesConfig;
    public ColorsConfig ColorsConfig => colorsConfig;
    #endregion
}
