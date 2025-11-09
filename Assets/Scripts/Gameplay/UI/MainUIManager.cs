using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    #region Enums
    public enum ScreenType
    {
        None,
        LevelCompleteScreen,
        LevelFailedScreen,
        GameplayScreen
    }
    #endregion

    #region Structures&Classes

    [Serializable]
    public class UIScreen
    {
        public ScreenType ScreenType;
        public GameObject screen;
    }
    #endregion

    #region Fields
    [SerializeField] UIScreen[] screens;

    UIScreen currentScreen;
    #endregion
   

    public void OpenScreen(ScreenType screenType)
    {
        if(currentScreen!=null) currentScreen.screen.SetActive(false);

        if (ScreenByType(screenType, out var screen))
        {
            currentScreen = screen;
            screen.screen.SetActive(true);        
        }
    }

    bool ScreenByType(ScreenType screenType, out UIScreen screen)
    {
        screen = screens.FirstOrDefault(s => s.ScreenType == screenType);
        return screen != null;
    }
}
