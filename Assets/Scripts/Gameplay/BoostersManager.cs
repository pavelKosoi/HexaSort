using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostersManager : MonoBehaviour
{

    #region Structures&Classes
    [Serializable]
    public class BoosterInfoPopUp
    {
        public Image icon;
        public TextMeshProUGUI descriptionTxt;
        public CanvasGroup canvasGroup;

        public void Activate(bool active, BoosterConfig config = null)
        {
            if (active)
            {
                canvasGroup.gameObject.SetActive(true);
                canvasGroup.alpha = 0;
            }
            if (active && config != null)
            {
                icon.sprite = config.Icon;
                descriptionTxt.text = config.Description;            
            }
            canvasGroup.DOFade(active ? 1 : 0, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                if(!active) canvasGroup.gameObject.SetActive(false);
            });
        }

    }
    #endregion

    #region Fields
    [SerializeField] BoosterButton[] buttons;
    [SerializeField] BoosterInfoPopUp boosterInfoPopUp;

    BoosterBase activeBooster;
    #endregion

    #region Getters
    BoostersConfig boostersConfig => ConfigsManager.Instance.BoostersConfig;
    #endregion

    #region UnityMethodes

    private void Awake()
    {
        InitButtons();
    }

    #endregion

    #region Initing
    void InitButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Init(i < boostersConfig.Boosters.Length ? boostersConfig.Boosters[i] : null);            
        }
    }

    #endregion

    #region BossterPerforming
    public void ActivateBooster(BoosterConfig config)
    {
        if (activeBooster != null)
        {
            activeBooster.OnCancel();
            activeBooster = null;
        }
        else
        {
            GameManager.Instance.CameraController.MoveCamera(CameraController.CameraPointType.Booster);
            GameManager.Instance.CameraController.SetBackgroundColor(ConfigsManager.Instance.ColorsConfig.BoosterCameraBackgroundColor);
            GameManager.Instance.InputManager.SetInputMode<StackPickingState>();
        }

        boosterInfoPopUp.Activate(true, config);
              
        switch (config.Type)
        {
            case BoosterType.Hammer:
                activeBooster = new HammerBooster(config);
                break;
            case BoosterType.Hand:
                activeBooster = new HandBooster(config);
                break;
            default:
                Debug.LogWarning("Unknown booster type");
                return;
        }

        activeBooster.OnActivate();
    }

    public void DeactivateBooster()
    {
        boosterInfoPopUp.Activate(false);
        GameManager.Instance.CameraController.MoveCamera(CameraController.CameraPointType.Default);
        GameManager.Instance.CameraController.SetBackgroundColor(ConfigsManager.Instance.ColorsConfig.DefaultCameraBackgroundColor);
        GameManager.Instance.InputManager.SetInputMode<DefaultInputState>();

        activeBooster?.OnDeactivate();
        activeBooster = null;
    }

    #endregion
}
